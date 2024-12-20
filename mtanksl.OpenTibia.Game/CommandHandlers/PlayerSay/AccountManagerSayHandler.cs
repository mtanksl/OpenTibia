using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

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

            WaitingForPasswordConfirmation,

            End
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

            WaitingForPlayerGenderConfirmation,

            End
        }

        private class AccountManagerState
        {
            public AccountManagerStateIndex Index { get; set; }

            public string Password { get; set; }

            public string PlayerName { get; set; }

            public Gender PlayerGender { get; set; }
        }

        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
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

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                            newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForAccountConfirmation;
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

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                            newAccountManagerState.Index = NewAccountManagerStateIndex.WaitingForPasswordConfirmation;
                        }
                        else if (newAccountManagerState.Index == NewAccountManagerStateIndex.WaitingForPasswordConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Your account '" + newAccountManagerState.Account + "' with password '" + newAccountManagerState.Password + "' has been created.") );

                                newAccountManagerState.Index = NewAccountManagerStateIndex.End;

                                //TODO: Create account
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

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                            accountManagerState.Index = AccountManagerStateIndex.WaitingForPasswordConfirmation;
                        }
                        else if (accountManagerState.Index == AccountManagerStateIndex.WaitingForPasswordConfirmation)
                        {
                            if (command.Message == "yes")
                            {
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Your password has been changed to '" + accountManagerState.Password + "'.") );

                                accountManagerState.Index = AccountManagerStateIndex.End;

                                //TODO: Change password
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

                            Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "'" + command.Message + "' are you sure?") );

                            accountManagerState.Index = AccountManagerStateIndex.WaitingForPlayerNameConfirmation;
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
                                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Your character '" + accountManagerState.PlayerName + "' has been created.") );

                                accountManagerState.Index = AccountManagerStateIndex.End;

                                //TODO: Create character
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

                return Promise.Completed;
            }

            return next();
        }
    }
}