using System;

namespace Account_Service.Helpers;

public static class LuhnAlgorithm
{
    public static int CalculateCheckDigit(string number)
    {
        int sum = 0;
        bool alternate = true;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(number.Substring(i, 1));

            if (alternate)
            {
                n *= 2;
                if (n > 9)
                {
                    n -= 9;
                }
            }

            sum += n;
            alternate = !alternate;
        }

        return (sum * 9) % 10;
    }
}
