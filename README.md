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
  * FEN
  * PGN
* Vlastní vzhled figurek  
 Každá druh figurky může mít vlastní vzhled 
* Dva módy časomíry  
  * Fixní délka tahu
  * Vzájemná
* Stockfish integrace (AI)  
* Atomic mode  
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

## Na čem se ještě pracuje:
* ### Ignácovy šachy 3D  
	* #### Šachovnice v reálném životě
	 ![Fyzická šachovnice][led-matrix]
	* Když hráč fyzicky zvedne figurku, zobrazí se mu dostupné tahy na svítící šachovnici
	* Fyzická tlačítka pro vzdání se, remízu, přítele na telefonu, restart hry
	* V případě hry sám se sebou lze jako protihráče vyzvat AI („přítel na telefonu"), která zvládne fyzicky pohyb na šachovnici provést
  
## Budoucí cíle:
* Optimalizace kódu; některé metody se volají vícekrát, než je nutné  
* Pojmenování proměnných a metod jsou někdy ne zcela jasná
* Překopání systému souřadnic      
 
  Místo int[] pro souřadnici použil Struct Point  
* Více popisků  
* Sjednotil jazyk, ve kterém je zdrojový kód
* Podpora pro více jazyků
* Vrácení více než jednoho tahu

[uvodni-fotka]: https://media.discordapp.net/attachments/1076565079333548184/1076565116658663495/2023-02-18_19_04_55-Ignacovy_sachy__Na_rade_je_Cerna..png
[led-matrix]: https://media.discordapp.net/attachments/572858382793310238/1067391638671982602/IMG_20230121_143202.jpg
