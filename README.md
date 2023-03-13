# SnailPass
<p align="center">
  <img src="https://user-images.githubusercontent.com/95579070/223737062-93cc4505-c52e-40bb-bee7-b822bd6cabb7.svg" width=25% height=25%>
</p>

SnailPass is a simple client-server password manager, that I'm writing with my colleagues to make for ourselves a convenient, custom environment for storing passwords.
Now is proposed to make the applications for two platforms. The client for each of the platforms is located in different repositories:
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
I intentionally used common SQL queries instead of EF as an experiment this time. :)

# Screenshots
#### Login interface
![ScrLogin](https://user-images.githubusercontent.com/95579070/224582764-527316b4-96fd-4bb4-b33e-f03699386b44.png)
#### Main window interface
![ScrGen](https://user-images.githubusercontent.com/95579070/224582767-afbb7d36-9eab-41fb-b9c4-c3e17f9b3aae.png)
![ScrNotes](https://user-images.githubusercontent.com/95579070/224582768-7273c1d3-d537-4b11-850e-0119a2a0cf64.png)
