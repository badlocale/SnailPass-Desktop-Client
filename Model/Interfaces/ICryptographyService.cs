using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass.Model.Interfaces
{
    public interface ICryptographyService
    {
        void Encrypt<ModelType>(ModelType model);
        Task EncryptAsync<ModelType>(ModelType model);
        void Decrypt<ModelType>(ModelType model);
        Task DecryptAsync<ModelType>(ModelType model);
    }
}
