   M S B A M L   `   `   `  ����  	Device001    Device001.W_Measurements   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  	Device001��5   	   $	��600��5   	   $	��600��6   $
��1400��5   	   $
��1400��6   ���	#FFE5E5E5.��5      �  � ColumnDefinitions 5	   
   �� 5
      $��* �6   �� 5      $
��Auto �6   �� 5      $	��430 �6   $	��430��6+    � RowDefinitions 5   
   �� 5      .+�5   
   �� $��0��6   $��Данные��5      .��5      �  5      �� 5      �� 5      $	��210 �6$   .+�5      7  1System.Windows.Forms.Integration.WindowsFormsHost  -   5          Name$ WinFH_Paint��5      $��0��5   #   $��Stretch=�5      $��Stretch=�6/   $��5,5,5,5q�5      � 5      $��1��6    5      �� 5      �� 5      $	��255 �6,   �� 5       $��75 �6+   �� 5!      $��0 �6+    5#      �� 5$      �� 5%      �� 5&      �� 5'      .+�5)      �� $��0��6    $��0��6-   $
��Left=�5*       $��5,5,5,5q�6;   $��28(�5+       PNCпектральная плотность потока возбуждения6Y   �� 5,      -   5         ��Name$ TB_OutExcitation��5-      $��0��5,      $��1��6+   $��Stretch=�5.      $��Center=�6<   $��2,2,2,2q�6W    �� TextWrapping$
 Wrap=�5/       �� 
IsReadOnly . $��28(�6D   .��6R   50      �� 51      $��0��6    $��2��6-   $
��Left=�52       $��5,5,5,5q�6;   $��28(�53       	Вт/м65   �� 54      $��1��6    $��0��6-   $
��Left=�55       $��5,5,5,5q�6;   $��28(�56       HFCпектральная плотность потока эмиссии6U   �� 57      -   5        $ TB_OutEmission��58      $��1��57      $��1��6+   $��Stretch=�59      $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5:       . $��28(�6D   .��6R   5;      �� 5<      $��1��6    $��2��6-   $
��Left=�5=       $��5,5,5,5q�6;   $��28(�5>       	Вт/м65   �� 5?      -   5         ��Name$ B_Save��5@      $��2��5?      $��1��6*   $��Сохранить��5A      {�.  $��28(�6?   $��Stretch=�5B      $��Center=�67   $��2,2,2,2q�6R   6c   �� 5C      -   5        $ B_Free��5D      $��3��5C      $��1��6*   $��
Сброс��5E      {�.  $��28(�6;   $��Stretch=�5F      $��Center=�67   $��2,2,2,2q�6R   6c   5G      5H      5I      �� 5J   
   $��2��6   $��Параметры��5K      .��5L      � $��5,5,5,5q�6    5M      �� 5N      $
��Auto �6$   �� 5O      $
��Auto �6$   �� 5P      $��20 �6$   �� 5Q      $
��Auto �6$   �� 5R      $
��Auto �6$   �� 5S      $
��Auto �6$   �� 5T      �� 5U      $
��Auto �6$   �� 5V      $
��Auto �6$   �� 5W      $
��Auto �6$   �� 5X      $
��Auto �6$   .+�5Z      � $��0��6    5[      �� 5\      $	��160 �6+   �� 5]       5_      �� 5`      �� 5a      .+�5c      �� $��0��6    $��0��6-   $
��Left=�5d      $��5,5,5,5q�6:   $��12(�5e      Подключение6<   �� 5f      -   5        $ B_D01��5g      $��0��5f      $��1��6+   $��Настройки D01��5h      $��Stretch=�5i      $��Center=�6;   $��2,2,2,2q�6V   6g   �� 5j      -   5        $ B_D02��5k      $��1��5j      $��1��6+   $��Настройки D02��5l      $��Stretch=�5m      $��Center=�6;   $��2,2,2,2q�6V   6g   5n      � 5o      $��1��6    5p      �� 5q      $	��160 �6+   �� 5r       5t      �� 5u      �� 5v      .+�5y      �� $��1��6    $��0��6-   $
��Left=�5z      $��5,5,5,5q�6:   $��12(�5{      Калибровка6;   �� 5|      -   5        $ B_Сalibration02��5}      $��1��5|      $��1��6+   $)��#Установка значений��5~      $��Stretch=�5      $��Center=�67   $��2,2,2,2q�6R   6c   5�      � 5�      -	   5         �Name$ Gr_OptionsD02��5�      $��3��5�      {�.   5�      �� 5�      $	��160 �6+   �� 5�       5�      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      �� 5�      .+�5�      �� $��1��6    $��0��6-   $
��Left=�5�       $��5,5,5,5q�6;   $��12(�5�       Экспорт в Exel6?   �� 5�      -
   5        	 ��Name$	 CB_TypeMeasurement��5�      $��1��5�      $��1��6-   $��Stretch=�5�      $��Center=�6=   $��2,2,2,2q�6X   �� 5�      $��3��6    $��0��6-   $y�	Collapsed=�6@   $
��Left=�5�       $��5,5,5,5q�6;   $��12(�5�       #!Шаг дискритизации6B   �� 5�      -   5        $	 CB_NumShift��5�      $��3��5�      $��1��6-   $y�	Collapsed=�6@   $��Stretch=�5�      $��Center=�6=   $��2,2,2,2q�6X   � 5�      $��5��6   $��2��5�       5�      �� 5�      �� 5�      �� 5�       5�      �� 5�      �� 5�      $��30 �6/   .+�5�      � $��0��6   $��0��6,    5�      �� 5�   "   �� 5�   "   .+�5�      � $��0��6#    5�   "   �� 5�   &   �� 5�   &   $��65 �68   �� 5�   &   $��35 �67    5�   "   �� 5�   &   .+�5�   "   �� -   5        
 ��Name$$
 T_MonochromatorStaticOrDynamic��5�   ,   $��0��5�   ,   $��0��69   $
��Left=�5�   ,   $��5,5,5,5q�6G   $��12(�5�   ,   Длина волны6G   �� 5�   "   -   5        $ TB_MonochromatorStatic��5�   *   $��0��5�   *   $��1��67   $��Stretch=�5�   *   $��Center=�6H   $��2,2,2,2q�6c   $
 Wrap=�5�   *   .��6>   5�   #   �� 5�   "   $��0��6,   $��2��69   $
��Left=�5�   %   $��5,5,5,5q�6@   $��12(�5�   %   нм68   5�      � 5�      $��1��6#   .+�5�   "   �  5�   &   �� 5�   *   �� 5�   *   .+�5�   &   �� $��0��60   $
��Left=�5�   0   $��5,5,5,5q�6K   $��12(�5�   0   +)Установленная решётка6V   �� 5�   &   -   5        $ 	 CB_MonochromatorStaticGrid��5�   /   $��1��5�   /   $��Stretch=�5�   /   $��Center=�6M   $��2,2,2,2q�6h   5�   #   5�      5�      �� 5�      -   5        $ B_WaveSattic��5�   !   $��0��5�   !   $��3��6.   $��->��5�   !   {�.  $��Stretch=�5�   !   $	��Top=�6?   $��2,2,2,2q�6W   6h   � 5�      $��1��6   $��0��6,    5�      �� 5�   "   �� 5�   "   .+�5�      � $��0��6#    5�   "   �� 5�   &   �� 5�   &   $��65 �68   �� 5�   &   $��35 �67    5�   "   �� 5�   &   �� 5�   &   �� 5�   &   .+�5�   "   �� $��0��6,   $��0��69   $
��Left=�5�   ,   $��5,5,5,5q�6G   $��12(�5�   ,   OMДлина волны несканирующего монохроматора6d   �� 5�   "   -   5        $ TB_MonochromatorDynamic��5�   *   $��0��5�   *   $��1��67   $��Stretch=�5�   *   $��Center=�6H   $��2,2,2,2q�6c   $
 Wrap=�5�   *   .��6>   5�   #   �� 5�   "   $��0��6,   $��2��69   $
��Left=�5�   ,   $��5,5,5,5q�6G   $��12(�5�   ,   нм6?   �� 5�   "   $��1��6,   $��0��69   $
��Left=�5�   ,   $��5,5,5,5q�6G   $��12(�5�   ,   Нижняя граница6J   �� 5�   "   -   5        $  TB_MonochromatorMinDynamic��5�   *   $��1��5�   *   $��1��67   $��Stretch=�5�   *   $��Center=�6H   $��2,2,2,2q�6c   $
 Wrap=�5   *   .��6>   5  #   �� 5  "   $��1��6,   $��2��69   $
��Left=�5  ,   $��5,5,5,5q�6G   $��12(�5  ,   нм6?   5     � 5     $��1��6#   .+�5  "   �  5	  &   �� 5
  *   �� 5  *   .+�5  &   �� $��0��60   $
��Left=�5  0   $��5,5,5,5q�6K   $��12(�5  0   +)Установленная решётка6V   �� 5  &   -   5        $!	 CB_MonochromatorDynamicGrid��5  /   $��1��5  /   $��Stretch=�5  /   $��Center=�6M   $��2,2,2,2q�6h   5  #   5     5     �� 5     -   5        $ 	B_Dynamic��5  !   $��1��5  !   $��3��6.   $��->��5  !   {�.  $��Stretch=�5  !   $	��Top=�6?   $��2,2,2,2q�6W   6h   5     5     � 5     $��4��6    5     �� 5     �� 5     �� 5      $��30 �6+    5"     �� 5#     �� 5$     �� 5%     .+�5'     �� $��1��6    $��0��6-   $
��Left=�5(      $��5,5,5,5q�6;   $��12(�5)      !Название образца6@   �� 5*     -   5        $ TB_Name��5+     $��1��5*     $��1��6+   {�.  $��Stretch=�5-     $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5.     .��62   5/     50     � 51     -   5        $ Gr_OptionsD01��52     $��5��51     {�.   54     �� 55     �� 56     $��65 �6,   �� 57     $��35 �6+   �� 58     $��40 �6+   �� 59     $��60 �6+    5;     �� 5<     �� 5=     �� 5>     $��15 �6(   �� 5?     �� 5@     �� 5A     �� 5B     �� 5C     $��15 �6(   �� 5D     �� 5E     .+�5G     �� $��0��6    $��3��6-   $
��Left=�5H      $��5,5,5,5q�6;   $��12(�5I      k=62   �� 5J     -   5        $
 TB_k��5K     $��0��5J     $��4��6+   $��Stretch=�5L     $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5M     .��62   5N     �� 5O     $��1��6    $��0��6-   $
��Left=�5P      $��5,5,5,5q�6;   $��12(�5Q      Напряжение ФЭУ6>   �� 5R     -   5        $ TB_PMT��5S     $��1��5R     $��1��6+   $��Stretch=�5T     $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5U     .��62   5V     �� 5W     $��1��6    $��2��6-   $
��Left=�5X      $��5,5,5,5q�6;   $��12(�5Y      В63   �� 5Z     -   5        $ B_PMT��5[  !   $��1��5Z     $��3��6*   $��->��5\  !   $��20(�6.   $��Stretch=�5]  !   $��Center=�6?   $��2,2,2,2q�6Z   6k   �� 5^     -   5        $ B_PMTxk��5_  !   $��1��5^     $��4��6*   $
��*k->��5`  !   $��20(�60   $��Stretch=�5a  !   $��Center=�6?   $��2,2,2,2q�6Z   6k   �� 5b     $��3��6    $��0��6-   $y�	Collapsed=�6>   $��Right=�5c      $��5,5,5,5q�6<   $��12(�5d      №61   �� 5e     -   5        $ TB_NumberRequest��5f     $��3��5e     $��1��6+   $y�	Collapsed=�6<   $��Stretch=�5g     $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5h      . .��6D   5i     �� 5j     $��4��6    $��0��6-   $
��Left=�5k      $��5,5,5,5q�6;   $��12(�5l      ФЭУ63   �� 5m     -   5        $ 	TB_PMTOut��5n     $��4��5m     $��1��6+   $��Stretch=�5o     $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5p      . .��6D   5q     �� 5r     $��5��6    $��0��6-   $y�	Collapsed=�6>   $
��Left=�5s      $��5,5,5,5q�6;   $��12(�5t      Опорный67   �� 5u     -   5        $ TB_ReferenceOut��5v     $��5��5u     $��1��6+   $y�	Collapsed=�6<   $��Stretch=�5w     $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5x      . .��6D   5y     �� 5z     $��6��6    $��0��6-   $
��Left=�5{      $��5,5,5,5q�6;   $��12(�5|      
Зонд64   �� 5}     -   5        $ TB_ProbeOut��5~     $��6��5}     $��1��6+   $��Stretch=�5     $��Center=�6<   $��2,2,2,2q�6W   $
 Wrap=�5�      . .��6D   5�     5�     � 5�     $��8��6    5�     �� 5�     �� 5�     �� 5�     �� 5�     .+�5�     �� -   5        $
 B_On��5�     $��0��5�     $��Подключение��5�     $��Stretch=�5�     $��Center=�67   $��2,2,2,2q�6R   6c   �� 5�     -   5        $ B_Correction��5�     $��1��5�     $��Корекция��5�     {�.  $��Stretch=�5�     $��Center=�67   $��2,2,2,2q�6R   6c   �� 5�     -    5        $ B_Start��5�     $��2��5�     $��
Старт��5�     {�.  $��Stretch=�5�     $��Center=�6;   $��2,2,2,2q�6V   6g   �� 5�     -!   5        $ B_Stop��5�     $��3��5�     $��Стоп��5�     {�.  $��Stretch=�5�     $��Center=�6;   $��2,2,2,2q�6V   6g   5�     5�     5�     �� 5�  
   $��1��6    �� ShowsPreview . $��3��5�  +   $��Center=�65   $��Stretch=�6R   5�     5�     