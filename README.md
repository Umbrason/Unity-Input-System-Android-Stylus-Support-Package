# Unity-Input-System-Android-Stylus-Support-Package

Adds support to the new Input System package for android stylus, that are not yet supported by unity (i.e. Samsung Spen)
# Caution
Android collects stylus events at a constant rate, that is independant from unity's framerate.
Therefore, the amount of **motion** events, that are raised per unity execution cycle, is limited, to prevent performance 'death spirals' that could arise otherwise. However to guarantee consistency with the styluses state, the most recent events are chosen and **button** events are always forwarded to the Input System.
