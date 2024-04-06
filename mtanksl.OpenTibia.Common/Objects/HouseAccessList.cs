using System;

namespace OpenTibia.Common.Objects
{
    public class HouseAccessList
    {
        public string Text { get; set; }

        public void SetText(string text)
        {
            if (text == "")
            {
                Text = null;
            }
            else
            {
                Text = text;
            }
        }

        private static bool IsInvited(string[] lines, string playerName)
        {
            foreach (var line in lines)
            {
                if (line.StartsWith("#") || line.StartsWith("!") ) // #comment
                {
                    continue;
                }

                int asterisk = line.IndexOf("*"); 

                if (asterisk > 0 && asterisk == line.Length - 1) // play*
                {
                    if (playerName.StartsWith(line.Substring(0, asterisk) ) ) // player starts with play
                    {
                        return true;
                    }
                }

                int questionMark = line.IndexOf("?"); 

                if (questionMark > 0 && questionMark == line.Length - 1) // playe?
                {
                    if (playerName.Substring(0, playerName.Length - 1) == line.Substring(0, questionMark) ) // player - r = playe
                    {
                        return true;
                    }
                }

                if (playerName == line) // player
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsExcluded(string[] lines, string playerName)
        {
            foreach (var line in lines)
            {
                if (line.StartsWith("#") || !line.StartsWith("!") ) // #comment
                {
                    continue;
                }

                if (playerName == line.Substring(1) ) // !player
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(string playerName)
        {
            if (Text != null)
            {
                string[] lines = Text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length > 0)
                {
                    return IsInvited(lines, playerName) && !IsExcluded(lines, playerName);
                }
            }

            return false;
        }
    }
}