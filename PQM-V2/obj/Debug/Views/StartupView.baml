   M S B A M L   `   `   `  ????A  =PQM-V2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:PQM_V2.ViewsPQM_V2.Views  5          PQM_V2.Views.StartupView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:PQM_V2.Views     5      Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  67   -   5        ???#707070  J? InputBindings  5      ?? 5   
    ?? CommandParameter$ OpenFileDialog??5      
 ?? Key$ O??5       ?? 	Modifiers$
 Ctrlf?5       ?? Command 5      ?? *openFileDialogCommand+5      ?? 5   
   $ OpenFolderDialog??5      $ O??5      $ 
Ctrl+Shiftf?5       5      ?? *openFolderDialogCommand+5      ?? 5   
   $ navigateHome??5      $ Enter??5      $
 Ctrlf?5       5      ?? *navigateHomeCommand+5      .??5      ?  ? ColumnDefinitions 5   
   ?? 5      $??1* ?6   ?? 5      $??150px ?6   ?? 5      $
??10px ?6   ?? 5       $??150px ?6   ?? 5!      $
??10px ?6   ?? 5"      $??150px ?6   ?? 5#      $??1* ?6    ? RowDefinitions 5%   
   ?? 5&      $??1* ?6   ?? 5'      $??2* ?6   ?? 5(      $??3* ?6   ?? 5)      $
??1.5* ?6   ?? 5*      $??3* ?6   .+?5,   
   ?? $??0??5-      $??0??6   $??7??5.      $??20, 0, 0, 0q?50      ???#D1D1D1??5/      ?? *smallFontSize+Welcome To PQM Automation App53      ?? 54   
   $??2??55      $??1??6   $??Center=?57      ???#ffffff??56      ?? *largeFontSize+	Getting?? 59      	Started5;      ?? 5<   
   $??3??5=      $??1??6   $??Center=?5?      ???#ffffff??5>      ?? *mediumFontSize+Load Data To?? 5A      Start Graphing5C      "  FontAwesome.WPF.ImageAwesome  5E   
   -   5           Icon$
 File=?5E      $??2??5F      $??3??6   $??20q?5G         
Foreground ?#ffffff  5J      d? 5K      $ OpenFileDialog??5L      	 d? MouseAction$	 	LeftClicke?5M       5K      ?? *openFileDialogCommand+5N      5P      ?? 5Q      $??3??5R      $??3??6   
 ?? TextAlignment$
 Center=?5S      $??Center=?5T      ???#ffffff??5U      ?? *mediumFontSize+
Open Files?? 5W      
Ctrl + O5Y        5\   
   -   5        $ Folder=?5\      $??2??5]      $??5??6   $??20q?5^       ?#ffffff  5a      d? 5b      $ OpenFolderDialog??5c      $	 	LeftClicke?5d       5b      ?? *openFolderDialogCommand+5e      5h      ?? 5i      $??3??5j      $??5??6   $
 Center=?5k      $??Center=?5l      ???#ffffff??5m      ?? *mediumFontSize+
Open Files?? 5o      Ctrl + Shift + O5q      5s      5t      