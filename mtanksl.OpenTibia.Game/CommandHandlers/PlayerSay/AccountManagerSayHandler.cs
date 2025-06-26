using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenTibia.Game.CommandHandlers
{
    public class AccountManagerSayHandler : CommandHandler<PlayerSayCommand>
    {
        private enum NewAccountManagerStateIndex
        {
            Start,

            AccountWaitingForAccount,

            AccountWaitingForAccountConfirmation,

            PasswordWaitingForPassword,

            PasswordWaitingForPasswordConfirmation
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

            PasswordWaitingForPassword,

            PasswordWaitingForPasswordConfirmation,

            CharacterWaitingForPlayerName,

            CharacterWaitingForPlayerNameConfirmation,

            CharacterWaitingForPlayerGender,

            CharacterWaitingForPlayerGenderConfirmation,

            NameWaitingForPlayerName,

            NameWaitingForPlayerNameConfirmation,

            NameWaitingForPlayerNewName,

            NameWaitingForPlayerNewNameConfirmation,

            GenderWaitingForPlayerName,

            GenderWaitingForPlayerNameConfirmation,

            GenderWaitingForPlayerNewGender,

            GenderWaitingForPlayerNewGenderConfirmation,

            MoveWaitingForPlayerName,

            MoveWaitingForPlayerNameConfirmation
        }

        private class AccountManagerState
        {
            public AccountManagerStateIndex Index { get; set; }

            public string Password { get; set; }

            public string PlayerName { get; set; }

            public string PlayerNewName { get; set; }

            public Gender PlayerGender { get; set; }
        }

        public override async Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Player.Rank == Rank.AccountManager)
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Say, command.Message) );

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
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                            newAccountManagerState.Index = NewAccountManagerStateIndex.Start;
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.Start)
                        {
                            if (command.Message == "account")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like your account to be?") );
                                
                                newAccountManagerState.Index = NewAccountManagerStateIndex.AccountWaitingForAccount;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.Start;
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.AccountWaitingForAccount)
                        {
                            newAccountManagerState.Account = command.Message;

                            if (newAccountManagerState.Account == Context.Server.Config.LoginAccountManagerAccountName || !IsValidAccountName(newAccountManagerState.Account) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This account is not valid, it must have at least 6 characters with letters or numbers. What would you like your account to be?") );
                                
                                newAccountManagerState.Index = NewAccountManagerStateIndex.AccountWaitingForAccount;
                            }
                            else
                            { 
                                using (var database = Context.Server.DatabaseFactory.Create() )
                                {
                                    var dbAccount = await database.AccountRepository.GetAccountByName(newAccountManagerState.Account);

                                    if (dbAccount != null)
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This account already exists. What would you like your account to be?") );
                                
                                        newAccountManagerState.Index = NewAccountManagerStateIndex.AccountWaitingForAccount;
                                    }
                                    else
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );

                                        newAccountManagerState.Index = NewAccountManagerStateIndex.AccountWaitingForAccountConfirmation;
                                    }
                                }
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.AccountWaitingForAccountConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like your password to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.PasswordWaitingForPassword;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like your account to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.AccountWaitingForAccount;
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.PasswordWaitingForPassword)
                        {
                            newAccountManagerState.Password = command.Message;

                            if ( !IsValidPassword(newAccountManagerState.Password) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This password is not valid, it must have at least 6 characters. What would you like your password to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.PasswordWaitingForPassword;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.PasswordWaitingForPasswordConfirmation;
                            }
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.PasswordWaitingForPasswordConfirmation)
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

                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your account '" + newAccountManagerState.Account + "' with password '" + newAccountManagerState.Password + "' has been created.") );
                                }
                                catch
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "There was a problem while creating your account, please try again.") );
                                }

                                newAccountManagerState.Index = NewAccountManagerStateIndex.Start;                                                                    
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like your password to be?") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.PasswordWaitingForPassword;
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
                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                            accountManagerState.Index = AccountManagerStateIndex.Start;
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.Start)
                        {
                            if (command.Message == "account")
                            {
                                StringBuilder message = new StringBuilder("Do you want to change your 'password', add a 'character'");

                                if ( !Context.Server.Config.GameplayAllowClones)
                                {
                                    if (Context.Server.Config.LoginAccountManagerAllowChangePlayerName)
                                    {
                                        message.Append(", change a character 'name'");
                                    }

                                    if (Context.Server.Config.LoginAccountManagerAllowChangePlayerGender)
                                    {
                                        message.Append(", change a character 'gender'");
                                    }

                                    message.Append(", 'move' a character to the temple");
                                }
                                      
                                message.Append("?");

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, message.ToString() ) );
                                
                                accountManagerState.Index = AccountManagerStateIndex.WaitingForAction;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

                                accountManagerState.Index = AccountManagerStateIndex.Start;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForAction)
                        {
                            if (command.Message == "password")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like your password to be?") );

                                accountManagerState.Index = AccountManagerStateIndex.PasswordWaitingForPassword;
                            }
                            else if (command.Message == "character")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerName;
                            }
                            else if (command.Message == "name" && !Context.Server.Config.GameplayAllowClones && Context.Server.Config.LoginAccountManagerAllowChangePlayerName)
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Which character?") );

                                accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerName;
                            }
                            else if (command.Message == "gender" && !Context.Server.Config.GameplayAllowClones && Context.Server.Config.LoginAccountManagerAllowChangePlayerGender)
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Which character?") );

                                accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerName;
                            }
                            else if (command.Message == "move" && !Context.Server.Config.GameplayAllowClones)
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Which character?") );

                                accountManagerState.Index = AccountManagerStateIndex.MoveWaitingForPlayerName;
                            }
                            else
                            {
                                StringBuilder message = new StringBuilder("Do you want to change your 'password', add a 'character'");

                                if ( !Context.Server.Config.GameplayAllowClones)
                                {
                                    if (Context.Server.Config.LoginAccountManagerAllowChangePlayerName)
                                    {
                                        message.Append(", change a character 'name'");
                                    }

                                    if (Context.Server.Config.LoginAccountManagerAllowChangePlayerGender)
                                    {
                                        message.Append(", change a character 'gender'");
                                    }

                                    message.Append(", 'move' a character to the temple");
                                }
                                      
                                message.Append("?");

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, message.ToString() ) );

                                accountManagerState.Index = AccountManagerStateIndex.WaitingForAction;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.PasswordWaitingForPassword)
                        {
                            accountManagerState.Password = command.Message;

                            if ( !IsValidPassword(accountManagerState.Password) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This password is not valid, it must have at least 6 characters. What would you like your password to be?") );

                                accountManagerState.Index = AccountManagerStateIndex.PasswordWaitingForPassword;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.PasswordWaitingForPasswordConfirmation;
                            }

                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.PasswordWaitingForPasswordConfirmation)
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

                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your password has been changed to '" + accountManagerState.Password + "'."));
                                }
                                catch
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "There was a problem while changing your password, please try again.") );
                                }

                                accountManagerState.Index = AccountManagerStateIndex.Start;                                  
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like your password to be?") );

                                accountManagerState.Index = AccountManagerStateIndex.PasswordWaitingForPassword;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.CharacterWaitingForPlayerName)
                        {
                            accountManagerState.PlayerName = command.Message;

                            if (accountManagerState.PlayerName == Context.Server.Config.LoginAccountManagerPlayerName || !IsValidPlayerName(accountManagerState.PlayerName) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character name is not valid, it must have at least 3 characters with letters. What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerName;
                            }
                            else
                            {
                                using (var database = Context.Server.DatabaseFactory.Create() )
                                {
                                    var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                    if (dbPlayer != null)
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character name already exists. What would you like as your character name?") );

                                        accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerName;
                                    }
                                    else
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );
                         
                                        accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerNameConfirmation;
                                    }
                                }
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.CharacterWaitingForPlayerNameConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerGender;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerName;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.CharacterWaitingForPlayerGender)
                        {
                            if (command.Message == "male")
                            {
                                accountManagerState.PlayerGender = Gender.Male;

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerGenderConfirmation;
                            }
                            else if (command.Message == "female")
                            {
                                accountManagerState.PlayerGender = Gender.Female;

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerGenderConfirmation;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerGender;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.CharacterWaitingForPlayerGenderConfirmation)
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

                                            BaseSpeed = 220,

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
                        
                                            Capacity = 400 * 100, 

                                            MaxCapacity = 400 * 100,

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

                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your character '" + accountManagerState.PlayerName + "' has been created.") );
                                }
                                catch
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "There was a problem while creating your character, please try again.") );
                                }     
                                    
                                accountManagerState.Index = AccountManagerStateIndex.Start;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.CharacterWaitingForPlayerGender;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.NameWaitingForPlayerName)
                        {
                            accountManagerState.PlayerName = command.Message;

                            using (var database = Context.Server.DatabaseFactory.Create() )
                            {
                                var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                if (dbPlayer == null || dbPlayer.AccountId != command.Player.DatabaseAccountId)
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character was not found. Which character?") );

                                    accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerName;
                                }
                                else
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );
                         
                                    accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerNameConfirmation;
                                }
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.NameWaitingForPlayerNameConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerNewName;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Which character?") );

                                accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerName;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.NameWaitingForPlayerNewName)
                        {
                            accountManagerState.PlayerNewName = command.Message;

                            if (accountManagerState.PlayerNewName == Context.Server.Config.LoginAccountManagerPlayerName || !IsValidPlayerName(accountManagerState.PlayerNewName) )
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character name is not valid, it must have at least 3 characters with letters. What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerNewName;
                            }
                            else
                            {
                                using (var database = Context.Server.DatabaseFactory.Create() )
                                {
                                    var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerNewName);

                                    if (dbPlayer != null)
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character name already exists. What would you like as your character name?") );

                                        accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerNewName;
                                    }
                                    else
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );
                         
                                        accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerNewNameConfirmation;
                                    }
                                }
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.NameWaitingForPlayerNewNameConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Player player = Context.Server.GameObjects.GetPlayerByName(accountManagerState.PlayerName);

                                if (player != null)
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character is currently online and can't be changed at this time.") );
                                }
                                else
                                {
                                    try
                                    {
                                        using (var database = Context.Server.DatabaseFactory.Create() )
                                        {
                                            var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                            dbPlayer.Name = accountManagerState.PlayerNewName;

                                            await database.Commit();
                                        }

                                        player = Context.Server.GameObjectPool.GetPlayerByName(accountManagerState.PlayerName);

                                        if (player != null)
                                        {
                                            player.Name = accountManagerState.PlayerNewName;
                                        }

                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your character '" + accountManagerState.PlayerName + "' has been changed.") );
                                    }
                                    catch
                                    {
                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "There was a problem while changing your character, please try again.") );
                                    }                                    
                                }

                                accountManagerState.Index = AccountManagerStateIndex.Start;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "What would you like as your character name?") );

                                accountManagerState.Index = AccountManagerStateIndex.NameWaitingForPlayerNewName;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.GenderWaitingForPlayerName)
                        {
                            accountManagerState.PlayerName = command.Message;

                            using (var database = Context.Server.DatabaseFactory.Create() )
                            {
                                var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                if (dbPlayer == null || dbPlayer.AccountId != command.Player.DatabaseAccountId)
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character was not found. Which character?") );

                                    accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerName;
                                }
                                else
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );
                         
                                    accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerNameConfirmation;
                                }
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.GenderWaitingForPlayerNameConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerNewGender;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Which character?") );

                                accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerName;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.GenderWaitingForPlayerNewGender)
                        {
                            if (command.Message == "male")
                            {
                                accountManagerState.PlayerGender = Gender.Male;

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerNewGenderConfirmation;
                            }
                            else if (command.Message == "female")
                            {
                                accountManagerState.PlayerGender = Gender.Female;

                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );

                                accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerNewGenderConfirmation;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerNewGender;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.GenderWaitingForPlayerNewGenderConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Player player = Context.Server.GameObjects.GetPlayerByName(accountManagerState.PlayerName);

                                if (player != null)
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character is currently online and can't be changed at this time.") );
                                }
                                else
                                {
                                    player = Context.Server.GameObjectPool.GetPlayerByName(accountManagerState.PlayerName);

                                    if (player != null)
                                    {
                                        player.Gender = accountManagerState.PlayerGender;

                                        foreach (var pair in player.Outfits.GetIndexed().ToArray() )
                                        {
                                            OutfitConfig outfitConfig = Context.Server.Outfits.GetCorrespondingOutfitById(pair.Key);

                                            if (outfitConfig != null)
                                            {
                                                player.Outfits.RemoveOutfit(pair.Key);

                                                player.Outfits.SetOutfit(outfitConfig.Id, pair.Value);
                                            }
                                        }

                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your character '" + accountManagerState.PlayerName + "' has been changed.") );
                                    }
                                    else
                                    {
                                        try
                                        {
                                            using (var database = Context.Server.DatabaseFactory.Create() )
                                            {
                                                var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                                dbPlayer.Gender = (int)accountManagerState.PlayerGender;

                                                foreach (var dbPlayerOutfit in dbPlayer.PlayerOutfits.ToArray() )
                                                {
                                                    OutfitConfig outfitConfig = Context.Server.Outfits.GetCorrespondingOutfitById( (ushort)dbPlayerOutfit.OutfitId);

                                                    if (outfitConfig != null)
                                                    {
                                                        dbPlayer.PlayerOutfits.Remove(dbPlayerOutfit);

                                                        dbPlayer.PlayerOutfits.Add(new DbPlayerOutfit()
                                                        {
                                                            PlayerId = dbPlayer.Id,

                                                            OutfitId = (int)outfitConfig.Id,

                                                            OutfitAddon = dbPlayerOutfit.OutfitAddon
                                                        } );
                                                    }
                                                }

                                                await database.Commit();
                                            }

                                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your character '" + accountManagerState.PlayerName + "' has been changed.") );
                                        }
                                        catch
                                        {
                                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "There was a problem while changing your character, please try again.") );
                                        }
                                    }
                                }

                                accountManagerState.Index = AccountManagerStateIndex.Start;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Should your character be 'male' or 'female'?") );

                                accountManagerState.Index = AccountManagerStateIndex.GenderWaitingForPlayerNewGender;
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.MoveWaitingForPlayerName)
                        {
                            accountManagerState.PlayerName = command.Message;

                            using (var database = Context.Server.DatabaseFactory.Create() )
                            {
                                var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                if (dbPlayer == null || dbPlayer.AccountId != command.Player.DatabaseAccountId)
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character was not found. Which character?") );

                                    accountManagerState.Index = AccountManagerStateIndex.MoveWaitingForPlayerName;
                                }
                                else
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "'" + command.Message + "' are you sure?") );
                         
                                    accountManagerState.Index = AccountManagerStateIndex.MoveWaitingForPlayerNameConfirmation;
                                }
                            }
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.MoveWaitingForPlayerNameConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Player player = Context.Server.GameObjects.GetPlayerByName(accountManagerState.PlayerName);

                                if (player != null)
                                {
                                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "This character is currently online and can't be moved at this time.") );
                                }
                                else
                                {
                                    player = Context.Server.GameObjectPool.GetPlayerByName(accountManagerState.PlayerName);

                                    if (player != null)
                                    {
                                        player.Direction = Direction.South;

                                        player.Spawn = player.Town;

                                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your character '" + accountManagerState.PlayerName + "' has been moved.") );
                                    }
                                    else
                                    {
                                        try
                                        {
                                            using (var database = Context.Server.DatabaseFactory.Create() )
                                            {
                                                var dbPlayer = await database.PlayerRepository.GetPlayerByName(accountManagerState.PlayerName);

                                                dbPlayer.Direction = 2;

                                                dbPlayer.SpawnX = dbPlayer.TownX;

                                                dbPlayer.SpawnY = dbPlayer.TownY;

                                                dbPlayer.SpawnZ = dbPlayer.TownZ;

                                                await database.Commit();
                                            }

                                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Your character '" + accountManagerState.PlayerName + "' has been moved.") );
                                        }
                                        catch
                                        {
                                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "There was a problem while moving your character, please try again.") );
                                        }
                                    }
                                }

                                accountManagerState.Index = AccountManagerStateIndex.Start;
                            }
                            else
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.PrivateFrom, "Which character?") );

                                accountManagerState.Index = AccountManagerStateIndex.MoveWaitingForPlayerName;
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