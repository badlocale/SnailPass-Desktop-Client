using SnailPass_Desktop.API;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SnailPass_Desktop.ViewModel.Commands
{
    internal class RegistrationCommand : CommandBase
    {
        RegistrationViewModel _viewModel;
        IUserRepository _repository;
        IMasterPasswordEncryptor _encryptor;

        public RegistrationCommand(RegistrationViewModel viewModel, IUserRepository repository, IMasterPasswordEncryptor encryptor)
        {
            _viewModel = viewModel;
            _repository = repository;
            _encryptor = encryptor;
        }

        public override bool CanExecute(object? obj)
        {
            bool isValidData = false;
            if (IsEmailValid(_viewModel.Email) != false && _viewModel.Email.Length >= 5 && 
                _viewModel.Password != null && _viewModel.Password.Length >= 10)
            {
                isValidData = true;
            }
            return isValidData;
        }

        public override async void Execute(object? obj)
        {
            _viewModel.ErrorMessage = null;

            _viewModel.ID = Guid.NewGuid().ToString();
            string encryptedPassword = _encryptor.Encrypt(_viewModel.Password, _viewModel.Email, 200000);
            UserModel user = new UserModel(_viewModel.ID, _viewModel.Username, _viewModel.Email, _viewModel.Hint, _viewModel.Nonce, encryptedPassword);

            Task<HttpResponseMessage> postResponce = WebAPI.Post("/users", user);

            if (postResponce.Result.StatusCode == System.Net.HttpStatusCode.OK) //200
            {
                //Task<HttpResponseMessage> getResponce = WebAPI.Get("/users");
                //string getResponceBody = await getResponce.Result.Content.ReadAsStringAsync();
                //JObject userJObject = JObject.Parse(getResponceBody);
                //user = userJObject["User"].ToObject<UserModel>();

                _repository.Add(user); //TODO sync with server

                _viewModel.ErrorMessage = "You are registered now";
                return;
            }
            else //TODO else if
            {
                _viewModel.ErrorMessage = "User with such email already exists";
                return;
            }
        }

        public bool IsEmailValid(string emailaddress)
        {
            if (emailaddress == null)
            {
                return false;
            }

            try
            {
                new MailAddress(emailaddress);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}