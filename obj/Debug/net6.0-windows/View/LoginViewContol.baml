   M S B A M L   `   `   `  ????L  HSnailPass_Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:SnailPass_Desktop.ViewSnailPass_Desktop.View  5      S.clr-namespace:SnailPass_Desktop.CustomControls SnailPass_Desktop.CustomControls  5      ,    &SnailPass_Desktop.View.LoginViewContol   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:SnailPass_Desktop.View     5      IcustomControls.clr-namespace:SnailPass_Desktop.CustomControls     5      .??5
      ??   ?? Orientation$  Vertical=?6   $??1??5      $	??220??5      $??0, 10, 0, 0q?5      .+?5   
   ?? $??	SnailPass??6      LightTextColor#??[  5      $??30(?5      $??
Uncut Sans)?5      $??DemiBold"?5      $??Center=?5      ?? 5   
     Logo#??[ 6   $	??100??5      $	??160??5      $??-10, 0, 0, 0q?5      ?? 5   
   $??E-mail??6     HeaderTextStyle#??[ 5      ?? 5   
   -   5         ??Name$ Email??5        OpenTextBox#??[ 5       ??5      ?? *Email+ ?? UpdateSourceTrigger$ PropertyChanged=?.??5    C   ??5!      ?? 5"        MailIcon ?? ImageSource# [ 6    j? Stretch$ Uniform=?5#       j? 
AlignmentX$
 Left=?5$      .??5%   "   5&      ?? 5(   
   $??Master password??6   #??[ 5)      :   4SnailPass_Desktop.CustomControls.BindablePasswordBox  5,   
   $??22??5.   -   $??
0, 5, 0, 0q?5/   -      Password 5,   -   ?? *
Password+ ?? Mode$ TwoWay=?$ PropertyChanged=?5/   -   ?? 51   
   $??36??52        ImportantTextStyle#??[ 53      ??51      ?? *ErrorMessage+53      ?? 55   
   $??60, 0, 60, 0q?6   ._?56      ?? -   5         ??Name$ 
btnLoginIn??56        	BigButton#??[ 57      $??LOGIN??59      ??58      ?? *LoginCommand+5:      5;      ?? 5=   
   -   5        $ 	btnSingUp??5=        
LinkButton#??[ 5>      $??Sing up??5@      $??80??5A      $??0, 15, 0, 0q?5B      ??5?      ?? *NavigateRegistrationCommand+5B      5D      5E      