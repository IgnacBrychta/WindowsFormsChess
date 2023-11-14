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
	 ![Fyzická šachovnice][led-matrix]
	 ![Box pro elektroniku pro šachovnici][skatule]
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
## Kolik času už tomu bylo obětováno?
Vývoj začal 29.11.2022 a pokračuje doposud, 18.2.2023. Kdyby se to celkem stihlo do tří a půl měsíců, bylo by to super. Celkem tomuto projektu bylo věnováno možná tak 200, 250, 300 hodin? Kdo ví už honestly.

[uvodni-fotka]: https://media.discordapp.net/attachments/1076565079333548184/1076565116658663495/2023-02-18_19_04_55-Ignacovy_sachy__Na_rade_je_Cerna..png
[led-matrix]: https://media.discordapp.net/attachments/1076565079333548184/1168549193028210728/2023-10-30_14_56_57-Autodesk_Fusion_360_Education_License.png?ex=6564a03e&is=65522b3e&hm=50bbcf66023752f8b831b46a172dcc397aa1b91afca827b50bda8fdd64be7812&=&width=1655&height=988
[skatule]: https://media.discordapp.net/attachments/1076565079333548184/1171026380104945705/IMG_0534.jpg?ex=656468cd&is=6551f3cd&hm=24e937edec645e7b208fbf2531ff6e649402d8c8d352a15cb9575e117cc87038&=&width=741&height=988
[fen-chesscom]: https://www.chess.com/terms/fen-chess
[pgn-chesscom]: https://www.chess.com/terms/chess-pgn
[nastaveni]: https://cdn.discordapp.com/attachments/1076565079333548184/1076574207607066624/2023-02-18_19_41_21-Nastaveni.png
[stockfish]: https://github.com/official-stockfish/Stockfish
