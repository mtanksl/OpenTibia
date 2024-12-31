using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Text.RegularExpressions;

namespace OpenTibia.Game.CommandHandlers
{
    public class AccountManagerSayHandler : CommandHandler<PlayerSayCommand>
    {
        private enum NewAccountManagerStateIndex
        {
            Start,

            WaitingForAccount,

            WaitingForAccountConfirmation,

            WaitingForPassword,

            WaitingForPasswordConfirmation
        }

        private class NewAccountManagerState
        {
            public NewAccountManagerStateIndex Index { get; set; }

            public string Account { get; set; }

            public string Password { get; set; }
        }

        private enum AccountManagerStateIndex
        {
            Start,

            WaitingForAction,

            WaitingForPassword,

            WaitingForPasswordConfirmation,

            WaitingForPlayerName,

            WaitingForPlayerNameConfirmation,

            WaitingForPlayerGender,

            WaitingForPlayerGenderConfirmation
        }

        private class AccountManagerState
        {
            public AccountManagerStateIndex Index { get; set; }

            public string Password { get; set; }

            public string PlayerName { get; set; }

            public Gender PlayerGender { get; set; }
        }

        public override async Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Player.Rank == Rank.AccountManager)
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, command.Message) );

                switch (command.Player.Client.AccountManagerType)
                {
                    case AccountManagerType.NewAccountManager:
                    {
                        NewAccountManagerState newAccountManagerState;

                        if ( !command.Player.Client.Data.TryGetValue("NewAccountManagerState", out var state) )
                        {
                            newAccountManagerState = new NewAccountManagerState();

                            command.Player.Client.Data.Add("NewAccountManagerState", newAccountManagerState);
                        }
                        else
                        {
                            newAccountManagerState = (NewAccountManagerState)state;
                        }

                        if (command.Message == "cancel")
                        {
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                            newAccountManagerState.Index = NewAccountManagerStateIndex.Start;
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.Start)
                        {
                            if (command.Message == "account")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like your account to be?") );
                                
                                newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForAccount;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.Start;
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.WaitingForAccount)
                        {
                            newAccountManagerState.Account = command.Message;

                            if (newAccountManagerState.Account == Context.Server.Config.LoginAccountManagerAccountName || !IsValidAccountName(newAccountManagerState.Account) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "This account is not valid, it must have at least 6 characters with letters or numbers. What would you like your account to be?") );
                                
                                newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForAccount;
                            }
                            else
                            { 
                                using (var database = Context.Server.DatabaseFactory.Create() )
                                {
                                    var dbAccount = await database.AccountRepository.GetAccountByName(newAccountManagerState.Account);

                                    if (dbAccount != null)
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "This account already exists. What would you like your account to be?") );
                                
                                        newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForAccount;
                                    }
                                    else
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                                        newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForAccountConfirmation;
                                    }
                                }
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.WaitingForAccountConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like your password to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForPassword;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like your account to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForAccount;
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.WaitingForPassword)
                        {
                            newAccountManagerState.Password = command.Message;

                            if ( !IsValidPassword(newAccountManagerState.Password) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "This password is not valid, it must have at least 6 characters. What would you like your password to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForPassword;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForPasswordConfirmation;
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.WaitingForPasswordConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                try
                                {
                                    using (var database = Context.Server.DatabaseFactory.Create() )
                                    {
                                        database.AccountRepository.AddDbAccount(new DbAccount()
                                        {
                                            Name = newAccountManagerState.Account,

                                            Password = newAccountManagerState.Password,

                                            PremiumUntil = null
                                        } );

                                        await database.Commit();
                                    } 

                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Your account '" + newAccountManagerState.Account + "' with password '" + newAccountManagerState.Password + "' has been created.") );
                                }
                                catch
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "There was a problem while creating your account, please try again.") );
                                }

                                newAccountManagerState.Index = NewAccountManagerStateIndex.Start;                                                                    
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like your password to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForPassword;
                            }
                        }
                    }
                    break;

                    case AccountManagerType.AccountManager:
                    { 
                        AccountManagerState accountManagerState;

                        if ( !command.Player.Client.Data.TryGetValue("AccountManagerState", out var state) )
                        {
                            accountManagerState = new AccountManagerState();

                            command.Player.Client.Data.Add("AccountManagerState", accountManagerState);
                        }
                        else
                        {
                            accountManagerState = (AccountManagerState)state;
                        }

                        if (command.Message == "cancel")
                        {
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                            accountManagerState.Index = AccountManagerStateIndex.Start;
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.Start)
                        {
                            if (command.Message == "account")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Do you want to change your 'password' or add a 'character'?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForAction;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                                accountManagerState.Index = AccountManagerStateIndex.Start;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForAction)
                        {
                            if (command.Message == "password")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like your password to be?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPassword;
                            }
                            else if (command.Message == "character")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerName;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Do you want to change your 'password' or add a 'character'?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForAction;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForPassword)
                        {
                            accountManagerState.Password = command.Message;

                            if ( !IsValidPassword(accountManagerState.Password) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "This password is not valid, it must have at least 6 characters. What would you like your password to be?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPassword;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPasswordConfirmation;
                            }

                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForPasswordConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                try
                                {
                                    using (var database = Context.Server.DatabaseFactory.Create() )
                                    {
                                        var dbAccount = await database.AccountRepository.GetAccountById(command.Player.DatabaseAccountId);

                                        dbAccount.Password = accountManagerState.Password;

                                        await database.Commit();
                                    }  

                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Your password has been changed to '" + accountManagerState.Password + "'."));
                                }
                                catch
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "There was a problem while changing your password, please try again.") );
                                }

                                accountManagerState.Index = AccountManagerStateIndex.Start;                                  
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like your password to be?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPassword;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForPlayerName)
                        {
                            accountManagerState.PlayerName = command.Message;

                            if (accountManagerState.PlayerName == Context.Server.Config.LoginAccountManagerPlayerName || !IsValidPlayerName(accountManagerState.PlayerName) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "This character name is not valid, it must have at least 3 characters with letters. What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerName;
                            }
                            else
                            {
                                using (var database = Context.Server.DatabaseFactory.Create() )
                                {
                                    var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                    if (dbPlayer != null)
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "This character name already exists. What would you like as your character name?") );

                                        accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerName;
                                    }
                                    else
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );
                         
                                        accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerNameConfirmation;
                                    }
                                }
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForPlayerNameConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerGender;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerName;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForPlayerGender)
                        {
                            if (command.Message == "male")
                            {
                                accountManagerState.PlayerGender = Gender.Male;

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerGenderConfirmation;
                            }
                            else if (command.Message == "female")
                            {
                                accountManagerState.PlayerGender = Gender.Female;

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerGenderConfirmation;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerGender;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForPlayerGenderConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                try
                                {
                                    using (var database = Context.Server.DatabaseFactory.Create() )
                                    {
                                        var dbAccount = await database.AccountRepository.GetAccountById(command.Player.DatabaseAccountId);

                                        var dbWorld = await database.WorldRepository.GetWorldByName(Context.Server.Config.LoginAccountManagerWorldName);

                                        database.PlayerRepository.AddPlayer(new DbPlayer()
                                        {
                                            AccountId = dbAccount.Id,

                                            WorldId = dbWorld.Id,

                                            Name = accountManagerState.PlayerName, 
                        
                                            Health = 150, 
                        
                                            MaxHealth = 150, 
                        
                                            Direction = 2,

                                            BaseOutfitId = accountManagerState.PlayerGender == Gender.Male ? 128 : 136,

                                            OutfitId = accountManagerState.PlayerGender == Gender.Male ? 128 : 136,

                                            BaseSpeed = 220,
                        
                                            Speed = 220,

                                            SkillFist = 10,

                                            SkillClub = 10,

                                            SkillSword = 10,

                                            SkillAxe = 10,

                                            SkillDistance = 10,

                                            SkillShield = 10,

                                            SkillFish = 10,

                                            Experience = 0, 
                        
                                            Level = 1, 
                        
                                            Mana = 55,
                        
                                            MaxMana = 55, 
                        
                                            Soul = 100, 
                        
                                            Capacity = 40000, 
                        
                                            Stamina = 2520, 

                                            Gender = (int)accountManagerState.PlayerGender,

                                            Vocation = 0,

                                            Rank = 0,

                                            SpawnX = Context.Server.Config.LoginAccountManagerPlayerNewPosition.X, 
                        
                                            SpawnY = Context.Server.Config.LoginAccountManagerPlayerNewPosition.Y,
                        
                                            SpawnZ = Context.Server.Config.LoginAccountManagerPlayerNewPosition.Z, 
                        
                                            TownX = Context.Server.Config.LoginAccountManagerPlayerNewPosition.X, 
                        
                                            TownY = Context.Server.Config.LoginAccountManagerPlayerNewPosition.Y, 
                        
                                            TownZ = Context.Server.Config.LoginAccountManagerPlayerNewPosition.Z
                                        } );

                                        await database.Commit();
                                    } 

                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Your character '" + accountManagerState.PlayerName + "' has been created.") );
                                }
                                catch
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "There was a problem while creating your character, please try again.") );
                                }     
                                    
                                accountManagerState.Index = AccountManagerStateIndex.Start;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerGender;
                            }
                        }
                    }
                    break;

                    default:

                        throw new NotImplementedException();
                }

                await Promise.Completed; return;
            }

            await next(); return;
        }

        private static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6 || password.Length > 29 || !IsValidISO88591(password) )
            {
                return false;
            }

            return true;
        }

        private static bool IsValidAccountName(string accountName)
        {
            if (string.IsNullOrEmpty(accountName) || accountName.Length < 6 || accountName.Length > 29 || !IsValidISO88591(accountName) || !Regex.IsMatch(accountName, "^[a-zA-Z0-9]+$") )
            {
                return false;
            }

            return true;
        }

        private static bool IsValidPlayerName(string playerName)
        {
            if (string.IsNullOrEmpty(playerName) || playerName.Length < 3 || playerName.Length > 29 || !IsValidISO88591(playerName) || !Regex.IsMatch(playerName, "^[a-zA-Z]+(?:[ '][a-zA-Z]+)*$") || playerName.StartsWith("god ", StringComparison.OrdinalIgnoreCase) || playerName.StartsWith("cm ", StringComparison.OrdinalIgnoreCase) || playerName.StartsWith("gm ", StringComparison.OrdinalIgnoreCase) )
            {
                return false;
            }

            return true;
        }

        private static bool IsValidISO88591(string input)
        {
            foreach (char c in input)
            {
                if (c > 255)
                {
                    return false;
                }
            }

            return true;
        }
    }
}