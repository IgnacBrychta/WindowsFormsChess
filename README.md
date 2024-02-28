# Ignácovy šachy
![Hra:][uvodni-fotka]
#### Šachy naprogramované v C# běžící díky Forms GUI  
## Co funguje:  
* Pohyby všech figurek
* Promování pěšáků  
* Vyhazování  
* Šachování  
* Remíza 
  * Čas vypršel
  * Příliš pohybů bez vyhození
  * Hráč se nemá jak pohnout a jeho král není šachován
* Locking
   
   Figurka se nemůže pohnout, pokud by její pohyb v určitém směru ohrozil krále
* Obětování figurek (záchrana krále)  
* En Passant  
* Skóre  
* Rošády  
* Notace a její export  
  * [FEN][fen-chesscom] (Forsyth-Edwards Notation)
  *  [PGN][pgn-chesscom] (Portable Game Notation)
* Vlastní vzhled figurek  
 
  Každá druh figurky může mít vlastní vzhled 
 
* Dva módy časomíry  
  * Fixní délka tahu
  * Vzájemná
* [Stockfish][stockfish] integrace (AI)  
* Atomic mode  
* Nastavení před spuštěním programu
* Zvýraznění důležitých událostí
  * Zvýraznění posledního tahu
    * Světle zelená - odkud se figurka pohnula
    * Tmavě zelená - kam se figurka pohnula
  * Král šachován
	  * Červená
  * Obětavé figurky
	  * Tmavě fialová
* Vrátit tah
* Zvukové efekty
* ... a další věci  
  
    
      
## Nastavení:
![nastaveni]


## Na čem se ještě pracuje:
* ### Ignácovy šachy 3D  
	* #### Šachovnice v reálném životě
	 ![Fyzická šachovnice][led-matrix] ![Box pro elektroniku pro šachovnici][skatule]
	* Když hráč fyzicky zvedne figurku, zobrazí se mu dostupné tahy na svítící šachovnici
	* Fyzická tlačítka pro vzdání se, remízu, přítele na telefonu, restart hry
	* V případě hry sám se sebou lze jako protihráče vyzvat [AI][stockfish] („přítel na telefonu"), která zvládne fyzicky pohyb na šachovnici provést
  
## Budoucí cíle:
* Optimalizace kódu; některé metody se volají vícekrát, než je nutné  
* Pojmenování proměnných a metod jsou někdy ne zcela jasná
* Překopání systému souřadnic      
 
  Místo int[] pro souřadnici použil Struct Point  
* Více popisků  
* Sjednotil jazyk, ve kterém je zdrojový kód
* Podpora pro více jazyků
* Vrácení více než jednoho tahu

[uvodni-fotka]: https://i.imgur.com/y7X5yLC.gif
[led-matrix]: https://i.imgur.com/2l6L9Ot.png
[skatule]: https://i.imgur.com/rfwegqm.jpeg
[fen-chesscom]: https://www.chess.com/terms/fen-chess
[pgn-chesscom]: https://www.chess.com/terms/chess-pgn
[nastaveni]: https://i.imgur.com/kiMFi1G.png
[stockfish]: https://github.com/official-stockfish/Stockfish
