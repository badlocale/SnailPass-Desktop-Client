   M S B A M L   `   `   `  ????L  HSnailPass_Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:SnailPass_Desktop.ViewSnailPass_Desktop.View  5      S.clr-namespace:SnailPass_Desktop.CustomControls SnailPass_Desktop.CustomControls  5      4    .SnailPass_Desktop.View.RegistrationViewControl   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:SnailPass_Desktop.View     5      IcustomControls.clr-namespace:SnailPass_Desktop.CustomControls     5      .??5
      ?? $??1??6     ?? Orientation$  Vertical=?5      $
??auto??5      $??40, 10, 40, 0q?5      .+?5   
   ?? $??Username??6      HeaderTextStyle#??[  5      ?? 5   
   -   5         ??Name$ Username??5        OpenTextBox#??[ 5      ??5      ?? *
Username+ ?? UpdateSourceTrigger$ PropertyChanged=?.??5   C   ??5      ?? 5        AccountIcon ?? ImageSource# [ 6    j? Stretch$ Uniform=?5       j? 
AlignmentX$
 Left=?5      .??5   "   5      ?? 5   
   $??E-mail??6   #??[  5      ?? 5   
   -   5        $ Email??5      #??[ 5!      ??5       ?? *Email+$ PropertyChanged=?.??5!   C   ??5"      ?? 5#        MailIcon# [ 6   $ Uniform=?5$      $
 Left=?5%      .??5&   "   5'      ?? 5)   
   $??Master password??6   #??[  5*      :   4SnailPass_Desktop.CustomControls.BindablePasswordBox  5,   
   -   5          Name$ Password??5,   -   $??22??5/   -   $??
0, 5, 0, 0q?50   -      Password 5-   -   ?? *
Password+ ?? Mode$ TwoWay=?$ PropertyChanged=?50   -   ?? 52   
   $??Hint to your password??6   #??[  53      ?? 55   
   -   5        $
 Hint??55      #??[ 57      ??56      ?? *Hint+$ PropertyChanged=?.??57   K   ??58      ?? 59        HintIcon# [ 6   $ Uniform=?5:      $
 Left=?5;      .??5<   "   5=      ?? 5?   
   $??40??5@        ImportantTextStyle#??[ 5A      ??5?      ?? *ErrorMessage+5A      ?? 5C   
   $??60, 22, 60, 0q?6   ._?5D      ?? -   5        	 ??Name$	 
btnLoginIn??5D        	BigButton#??[ 5E      $??SIGN UP??5G      ??5F      ?? *RegistrationCommand+5H      5I      ?? 5K   
   -   5        $	 btnBackToLogin??5K        
LinkButton#??[ 5L      $??Back to login??5N      $	??110??5O      $??0, 15, 0, 0q?5P      ??5M      ?? *NavigateLoginCommand+5P      5Q      5R      