# Apollon

# Usage

Apollon wird über die Konsole und Konsole Arugmente gesteuert.

Die Hilfe kann über über die möglichen Befehlen kann über:

```
apolon.exe -h
```

angezeigt werden.

Ausführen eines Programs:

```
apolon.exe test.apo -q test(X).
```

Ausführen mit gesetzten Loglevel:

```
apolon.exe test.apo -q test(X). -l Silly
```

Dokumentation ausgeben:

```
apolon.exe test.apo -d
```

# Syntaktische Abweichungen

Bei uns ist der disunifications operator statt `\=` -> `!=`.
Sonst sollten die normale ASP Sytax eingehlaten werden, so wie sie von sasp unterstützt wird. Also keine Listen, Choise Rules, ...
