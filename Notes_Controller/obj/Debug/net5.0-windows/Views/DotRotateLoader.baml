   M S B A M L   `   `   `  ����  Notes_Controller,    &Notes_Controller.Views.DotRotateLoader   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   
   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5          � Title$  
MainWindow��5   	   $	��350��6   $	��400��6)   $	��350��65   $	��400��6E   ���	#7F4D6DF9 � WindowStyle$
 None=�6k    � 	Resources 5         EllipsColor��%+   &	        ��5	   
   	white��5
      5      .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $	��300 �6   .+�5   
   �� -   5         ��Name$ 	TextBlock��5      $��24(�6'   $��Center=�65   $��Center=�5      
������� � Triggers 5      :� 5       :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� .��5      ��  �� TargetProperty$ Foreground.Color�5   !    g� Duration$ 0:0:3D�5   !   	 g� RepeatBehavior$	 Forever��5   !   
 g� AutoReverse$

 True��5   !   g�5   &   ��  �� KeyTime$ 0:0:1��6:    �� Value$ White��6J   �� 5   &   $ 0:0:2��6:   $ Orange��6J   �� 5    &   $ 0:0:3��6:   $ Yellow��6J   5"      5#      5$      5%      5'      A� 5(   
   -   5         A�Name$	 El1��5(   6   $��1��6   $��20��6   $��20��6*   ��5)      �� 5*       �� Color# [  6"   5,      A� 5.   
   -   5        $	 El2��5.   6   $��1��6   $��20��6   $��20��6*   ��5/      �� 50      # [  6"   52      A� 53   
   -   5        $	 El3��53   6   $��1��6   $��20��6   $��20��6*   ��54      �� 55      # [  6"   57      A� 58   
   -   5        $	 El4��58   6   $��1��6   $��20��6   $��20��6*   ��59      �� 5:      # [  6"   5<      A� 5=   
   -   5        $	 El5��5=   6   $��1��6   $��20��6   $��20��6*   ��5>      �� 5?      # [  6"   5A      5C      5D      