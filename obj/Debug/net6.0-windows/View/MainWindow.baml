   M S B A M L   `   `   `  ????L  HSnailPass_Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:SnailPass_Desktop.ViewModelSnailPass_Desktop.ViewModel  5   	   ?$clr-namespace:SnailPass_Desktop.ViewSnailPass_Desktop.View  5   	   5clr-namespace:SnailPass_DesktopSnailPass_Desktop  5   	   "    SnailPass_Desktop.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?	viewModel)clr-namespace:SnailPass_Desktop.ViewModel     5   	   5view$clr-namespace:SnailPass_Desktop.View     5   	   1localclr-namespace:SnailPass_Desktop     5   	     ? Title$  
MainWindow??5
   	   $	??600??5   	   $
??1000??5   	   $	??100??5   	   $	??450??5   	   
??????  ? WindowStartupLocation$ CenterScreen=?5   	    ? WindowStyle$
 None=?5   	    ? 
ResizeMode$ CanResizeWithGrip=?5   	    ? AllowsTransparency$
 True??5   	   .??5      ?  ? ColumnDefinitions 5   
   ?? 5      $	??160??6   $	??200 ?6.   $	??250??6:   ?? 5       ? RowDefinitions 5   
   ?? 5      $??30 ?6   ?? 5      .+?5   
   ?? -   5           HeaderBorderStyle#??[  5      5      ?? 5    
     BodyBorderStyle#??[ 6   ? 5"   
   -   5        $??TopMenu??5"      $??0??5#      $??2??5$       5%      ?? 5&      $
??auto ?6'   ?? 5'      .+?5*      ?? $??0??6    ?? Orientation$ 
Horizontal=?5+      .+?5,      ??   TopMenuButton#??[ 6   $
??File??6S   ?? 5-      #??[ 6   $
??Edit??6S   ?? 5.      #??[ 6   $
??View??6S   ?? 5/      #??[ 6   $??Account??6S   ?? 50      #??[ 6   $??Window??6S   ?? 51      #??[ 6   $
??Help??6S   52      ?? 54      $??1??6   $ 
Horizontal=?55      $??RightToLeft=?56      $??0, 0, 10, 0q?57      .+?58      ?? -   5         ??Name$ btnClose??58        TopMenuSmallButton#??[ 59      $??13(?5:      $??X??5;      5<      ?? 5>      -   5        $ btnMaximize??5>      #??[ 5?      $??18(?5@      $??^??5A      5B      ?? 5D      -   5        $ btnMinimize??5D      #??[ 5E      $??18(?5F      $??-??5G      5H      5I      5J      ? 5L   
   -   5        $??	SlideMenu??5L      $??0??5M      $??1??5N      .+?5O      ?? $??
0, 0, 1, 0q?6   	 ?? CornerRadius$	 
0, 0, 0, 6??5P        BorderColor#??[ 5Q        SecondBackground#??[ 5R      ?? 5T      $ Vertical=?6   .+?5U      	? -   5        
 	?Name$
 HomePage??5U       	? 	GroupName$ LeftMenu??5V        LeftMenuButton#??[ 5X        LightTextColor
 ? Tag# [ 5Y      ??5W      ?? *NavigateHomeCommand+.??5Z      ?? $ 
Horizontal=?6!   .+?5[      ?? $??15, 0, 0, 0q?6$   $??22??5\   $   $??22??5]   $   #??[ 5^   $    J? OpacityMask 5_      !? 5`   "    j? Stretch$ Uniform=?6.     HomeVectorIcon !? Visual# [ 5a   .   5c      ?? 5e       	 LeftMenuButtonText#??[	 6$   $
??Home??5f   $   5g      5h      	? 5j      -   5        $
 PasswordsPage??5j      $ LeftMenu??5k      #??[ 5m      # [ 5n      ??5l      ?? *NavigateAccountsCommand+.??5o      ?? $ 
Horizontal=?6!   .+?5p      ?? $??15, 0, 0, 0q?6$   $??22??5q   $   $??22??5r   $   #??[ 5s   $    5t      !? 5u   "   $ Uniform=?6.    
 AccauntKeysVectorIcon# [
 5v   .   5x      ?? 5z      #??[	 6$   $??Accounts??5{   $   5|      5}      	? 5      -	   5        $
 	NotesPage??5      $ LeftMenu??5?      #??[ 5?      # [ 5?      ??5?      ?? *NavigateNotesCommand+.??5?      ?? $ 
Horizontal=?6!   .+?5?      ?? $??15, 0, 0, 0q?6$   $??22??5?   $   $??22??5?   $   #??[ 5?   $    5?      !? 5?   "   $ Uniform=?6.     NoteVectorIcon# [ 5?   .   5?      ?? 5?      #??[	 6$   $??Notes??5?   $   5?      5?      5?      5?      ?? 5?   
   -
   5         ??Name$ Content??5?      $??1??5?      $??1??5?      ._?5?      ?? ??6   ?? *CurrentViewModel+ ? 	Resources 5?       ?? DataType/  )SnailPass_Desktop.ViewModel.HomeViewModel,   &SnailPass_Desktop.View.HomeViewControl3  -SnailPass_Desktop.ViewModel.AccountsViewModel0   *SnailPass_Desktop.View.AccountsViewControl0  *SnailPass_Desktop.ViewModel.NotesViewModel-   'SnailPass_Desktop.View.NotesViewControl??%?   (??       5?   #   *, +)(?? 2     5?   #   *, +)(?? d     5?   #   *, +)?? 5?      "  6#   .R?5?        5?      ?? 5?      "  6#   .R?5?        5?      ?? 5?      "  6#   .R?5?        5?      5?      5?      5?      ?? 5?   
   $??0??6   $??2??5?       ?? ShowsPreview$ False??5?      $??4??5?      $??0, 0, -2, 0q?5?      $??Right=?5?      $??Stretch=?5?      
?????? 5?      5?      5?      