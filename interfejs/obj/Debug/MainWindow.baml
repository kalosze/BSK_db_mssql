   M S B A M L   `   `   `  ����D  @interfejs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:interfejs	interfejs  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       interfejs.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:interfejs     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	     �Language$  pl-PL�5
   I    � Title$ BSK MAC z MSSQL��6	   $	��400��6!   $	��525��6.   $	��525��6:   $	��400��6I   .��5      �  � RowDefinitions 5   
   �� 5      .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5      $��10,66,10,30q�6%      
IsReadOnly .    CanUserAddRows .   � 	Resources 5       ��Name   RowMenu � DataContext	 �� RelativeSource��%|   &	        �� 5      $ gridRowMenu��6    5      �� *PlacementTarget.DataContext+	 �� *Self+5      5      
 � ContextMenu
 5      �� 5      .@�5      m� -   5        $��	Odśwież��5      60   5         RowStyle 5      �� 5       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6   .�5      ��  �� Property �@�ContextMenu �� Value# [  64   5      5       �� 5!   
   -   5         ��Name$ wybieranieTabeli��5!      $
��Left=�6-   $��	10,34,0,0q�6H   $	��Top=�6[   $	��120��6s   {�.  5"      �� 5#   
   -   5         ��Name$ labelTabela��5#      $��Tabela:��6%   $
��Left=�67   $��10,8,0,0q�6R   $	��Top=�6d   �� 5$   
   -   5         ��Name$ dodawanieRekorduBtn��5$      $��Dodaj nowy rekord��6.   $��	10,0,0,10q�6J   $
��Left=�6]   $	��120��6x   $��20��6�   $��Bottom=�6�   {�.  6�   �� 5%   
   -   5        $ loginButton��5%      $��Zaloguj��6&   $	��Top=�68   $��Right=�6c   $��75��6   �� 5&   
   -   5        $! zarzadzanieUzytkownikamiBtn��5&      $!��Zarządzanie użytkownikami��66   $��0,0,80,0q�6Z   $	��Top=�6l   $��Right=�6�   $	��164��6�   $y�Hidden=�6�   {�.  6�   �� 5'   
   -   5        $ 	szukajBtn��5'      {�.  $��Wyszukaj��66   $
��Left=�6I   $��
163,34,0,0q�6d   $	��Top=�6x   $��75��6�   6�   �� 5(   
   -	   5        $ usunBtn��5(      {�.  $��Usuń rekord��64   $
��Left=�6J   $��
135,0,0,10q�6e   $��75��6y   $��20��6�   $��Bottom=�6�   �� 5)   
   -
   5        $ zarzadzanieTabelamiBtn��5)      $��Zarządzanie tabelami��61   $��	0,0,249,0q�6P   $	��Top=�6c   $��Right=�6�   $	��130��6�   $y�Hidden=�6�   {�.  6�   �� 5*   
   -   5        $ ktoZalogowany��5*      $�� ��6'   $��273,20,10,0q�62   $	��Top=�6G   $��Right=�6_   �� 5+   
   -   5        $ 	edytujBtn��5+      $��Edytuj rekord��6$   $
��Left=�6<   $��
215,0,0,10q�6W   $��75��6k   {�.  $��20��6�   $��Bottom=�6�   �� 5,   
   -   5        $ 	infoLabel��5,      $�� ��6#   $��
386,0,10,4q�6.   $��Right=�6B   $��26��6e   $��Bottom=�6q   5-      5.      