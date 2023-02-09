# Unity-Input-System-Android-Stylus-Support-Package

Adds support to the new Input System package for android stylus, that are not yet supported by unity (i.e. Samsung Spen)
#Caution:
Stylus motion events are limited per frame to prevent performance 'death spirals' that could arise from raising multiple device events per unity execution cycle, however to guarantee consistency button events are always forwarded to the Input System
