using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorChecker
{
    public class FieldChecker
    {
        /// <summary>
        /// Checks wether text field contains digits
        /// </summary>
        /// <param name="field"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public bool IsTextFieldCorrect(string field, int maxCount)
        {
            if (String.IsNullOrWhiteSpace(field))
            {
                return false;
            }

            if (field.Length > maxCount)
            {
                return false;
            }

            return Iterator(0, field, false);            
        }
        /// <summary>
        /// Checks wether text field contains letters
        /// </summary>
        /// <param name="field"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public bool IsPhoneCorrect(string field, int maxCount)
        {
            if (String.IsNullOrWhiteSpace(field))
            {
                return false;
            }

            if (field.Length > 20)
            {
                return false;
            }

            int ptr = 0;

            if (field[0].Equals('+'))
            {
                ptr = 1;
            }
            else
            {
                ptr = 0;
            }

            return Iterator(ptr, field, true);
            
        }

        /// <summary>
        /// Iterator. If Digit_Letter = true, string mustn't contain letters
        /// Otherwise string must be formed of digits only
        /// /// Method can be overrided
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="iterableString"></param>
        /// <param name="Digit_Letter"></param>
        /// <returns></returns>
        protected virtual bool Iterator(int ptr, string iterableString, 
            bool Digit_Letter )
        {
            for (;ptr < iterableString.Length; ptr++)
            {
                if (iterableString[ptr].Equals(' '))
                {
                    continue;
                }

                if (Digit_Letter)
                {
                    if (Char.IsLetter(iterableString[ptr]))
                    {
                        return false;
                    }
                }
                else
                {
                    if (Char.IsDigit(iterableString[ptr]))
                    {
                        return false;

                    }
                }
            }

            return true;
        }

    }
}
