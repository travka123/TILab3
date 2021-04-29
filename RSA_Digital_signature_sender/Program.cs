using System;
using System.Numerics;
using System.Text;

namespace RSA_Digital_signature_sender {

    public struct Keys {
        public BigInteger e;
        public BigInteger d;
        public BigInteger r;
    }

    class Program {

        public static BigInteger GetD(BigInteger fEuler, BigInteger e) {
            BigInteger d0 = fEuler, d1 = e, x0 = 1, x1 = 0, y0 = 0, y1 = 1, q, d2, x2, y2;
            while (d1 > 1) {
                q = d0 / d1;
                d2 = d0 % d1;
                x2 = x0 - q * x1;
                y2 = y0 - q * y1;
                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = y1;
                y1 = y2;
            }
            return (y1 > 0) ? y1 : y1 + fEuler;
        }

        public static bool IsCoprime(BigInteger num1, BigInteger num2) {
            BigInteger temp;
            while (num2 != 0) {
                num1 %= num2;
                temp = num2;
                num2 = num1;
                num1 = temp;
            }
            return num1 == 1;
        }

        public static Keys GetKeys(BigInteger q, BigInteger p) {
            BigInteger r = BigInteger.Multiply(q, p);
            BigInteger fEuler = BigInteger.Multiply(p - 1, q - 1);
            BigInteger e = 7;
            while (!IsCoprime(fEuler, e) && (fEuler > e)) {
                e++;
            }
            if (e == fEuler) {
                throw new Exception("Не удалось подобрать e");
            }
            

            BigInteger d = GetD(fEuler, e);
            
            return new Keys { d = d, e = e, r = r };
        }

        public static BigInteger GetHash(BigInteger[] message) {
            BigInteger n = 23 * 29;
            BigInteger h = 100;
            foreach (BigInteger val in message) {
                h = BigInteger.ModPow(h + val, 2, n);
            }
            return h;
        }

        public static BigInteger Encrypt(BigInteger message, BigInteger d, BigInteger r) {
            return BigInteger.ModPow(message, d, r);
        }

        static void Main(string[] args) {

            Console.Write("Введите p: ");
            BigInteger p = BigInteger.Parse(Console.ReadLine());
            Console.Write("Введите q: ");
            BigInteger q = BigInteger.Parse(Console.ReadLine());

            Console.Write("Введите сообщение: ");
            byte[] bytes = Encoding.ASCII.GetBytes(Console.ReadLine());
            BigInteger[] message = new BigInteger[bytes.Length];
            int i = 0;
            foreach (byte val in bytes) {
                message[i] = val;
                i++;
            }

            Keys keys;
            try {
                keys = GetKeys(q, p);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("Открытый ключ (e, r): " + keys.e.ToString() + ", " + keys.r.ToString());
            Console.WriteLine("Закрытый ключ (d, r): " + keys.d.ToString() + ", " + keys.r.ToString());
            BigInteger h = GetHash(message);
            Console.WriteLine("Цифровая подпись: " + Encrypt(h, keys.d, keys.r));
        }
    }
}
