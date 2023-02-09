# SnailPass
SnailPass is a simple client-server password manager, that i'm writing with my colleagues to make for ourselves a convenient, custom environment for storing passwords.
Now is proposed to make applications for two platforms. The client for each of the platforms is located in different repositories:
- Windows (you are here now)
- [Android](https://github.com/IlyaYDen/SnailPasswordManager)
<!-- -->
The application also has a [server part](https://github.com/rebmanop/SnailPass-REST-API), where user data will be stored.

# How it works
The main idea of the application is storing user's passwords and other information in encrypted form. Encryption is carried out using a symmetric algorithm with a hashed master password as a key. At the same time a key is not stored anywhere and is never transmitted via the network, the user just has to remember it. For this reason, the data can be decrypted only locally and the server stores only the cipher.
#### Cryptographic algorithms
- AES-CBC as a symmetric-key algorithm.
- Pbkdf2 as a key derivation function.
- SHA-512 as a hash function.
#### Libraries and frameworks
Application written using WPF framework (MVVM) and several external libraries:
- Autofac as DI container.
- Newtonsoft.JSON for simplifying JSON boilerplate.
- Serilog for logging.
- Microsoft.Data.Sqlite for manage data.
<!-- -->
I intentionally used common SQL queries instead EF as as an experiment this time. :)

# Screenshots
#### Login interface
![LoggingScreen](https://user-images.githubusercontent.com/95579070/204347287-8c84d3cf-d271-40c8-aab5-f13a84e7530d.png)
#### Main window interface
![image](https://user-images.githubusercontent.com/95579070/217944712-53c77519-634a-4cb5-b10c-dcf5b23058d2.png)
