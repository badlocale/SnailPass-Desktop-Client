using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model.Cryptography
{
    public static class CryptoConstants
    {
        private static readonly int _defaultEncKeyIterations = 100000;
        private static readonly int _defaultNetworkIterations = 120000;
        private static readonly int _defaultLocalIterations = 180000;

        public static readonly int ENC_KEY_ITERATIONS_COUNT;
        public static readonly int NETWORK_ITERATIONS_COUNT;
        public static readonly int LOCAL_ITERATIONS_COUNT;

        static CryptoConstants()
        {
            try
            {
                ENC_KEY_ITERATIONS_COUNT = int.Parse(ConfigurationManager.AppSettings["enc_key_hash_iterations"]);
            }
            catch
            {
                ENC_KEY_ITERATIONS_COUNT = _defaultEncKeyIterations;
            }

            try
            {
                NETWORK_ITERATIONS_COUNT = int.Parse(ConfigurationManager.AppSettings["network_hash_iterations"]);
            }
            catch
            {
                NETWORK_ITERATIONS_COUNT = _defaultNetworkIterations;
            }

            try
            {
                LOCAL_ITERATIONS_COUNT = int.Parse(ConfigurationManager.AppSettings["local_hash_iterations"]);
            }
            catch
            {
                LOCAL_ITERATIONS_COUNT = _defaultLocalIterations;
            }
        }
    }
}
