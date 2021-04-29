using System;
using System.Numerics;
using System.Text;

namespace RSA_Digital_signature_receiver {
    class Program {

        public static BigInteger GetHash(BigInteger[] message) {
            BigInteger n = 23 * 29;
            BigInteger h = 100;
            foreach (BigInteger val in message) {
                h = BigInteger.ModPow(h + val, 2, n);
            }
            return h;
        }

        public static BigInteger Decrypt(BigInteger message, BigInteger e, BigInteger r) {
            return BigInteger.ModPow(message, e, r);
        }

        static void Main(string[] args) {

            Console.Write("Введите e: ");
            BigInteger e = BigInteger.Parse(Console.ReadLine());
            Console.Write("Введите r: ");
            BigInteger r = BigInteger.Parse(Console.ReadLine());
            Console.Write("Введите сообщение: ");
            byte[] bytes = Encoding.ASCII.GetBytes(Console.ReadLine());
            BigInteger[] message = new BigInteger[bytes.Length];
            int i = 0;
            foreach (byte val in bytes) {
                message[i] = val;
                i++;
            }
            Console.Write("Введите электронную подпись: ");
            BigInteger signature = BigInteger.Parse(Console.ReadLine());

            if (GetHash(message) == Decrypt(signature, e, r)) {
                Console.WriteLine("Подпись верна");
            }
            else {
                Console.WriteLine("Подпись не верна");
            }
        }
    }
}
