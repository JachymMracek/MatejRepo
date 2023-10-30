Author - Jáchym Mraček
Command line arguments:
                     -m, --mode      Required. Jaký řežím chcete? trivial,random nebo pattern: OČEKÁVANÝ VSTUP JE TRIVIAL NEBO RANDOM NEBO PATTERN
                     -n, --name      Required. Jak chcete obrázek pojmenovat:
                     -w, --width     Required. Jakou šířku má mít obrázek: (doporučená hodnota je 4096)
                     -h, --height    Required. Jakou výšku má mít obrázek: (doporučená hodnota je 4096)              
Algorithms:
          - Pro generování trivial obrázku používám RGB barvy, tedy algoritmus běží přes tři cykly.
          - Pro generování random obrázku používám array, kde si vygeneruji všechny RGB barvy > následně promíchám náhodně barvy a postupně nanosím na paletu.
          - Pro generování čtvercového pattern obrázku si musím uvědomit, že počet pixelů je dán (2 * round + 1) ** 2, kde round je počet nanosených čtverců.
            Můj postup funguje, tak že nejdříve nanosim horní část čtverce, potom dolní, potom levou a potom pravou. V kodů je postup popsán i s komenty. Poslední čtverec není kompletní,
            a proto nanosím jenom horní a pravou část. Takto využiju všech 4096*4096 RGB barev.
Popis kodu:
          - Kod obsahuje dvě třídy Options a Picture, kde Options zařituje správu příkazové řádky a Picture generuje obrázek. Picture obsahuje tři funkce:
          -GenerateTrivialPicture(),GenerateRandomPicture() a GeneratePatternPicture() s pomocnou GetRGBpixel(), která zajišťuje správnou volbu barev.
Output:
       - Typ obrázku se zadaným pojmenováním a zadanou velikostí. V případě malého počtu pixelů program vyhodí chybovou hlášku.
       - V případě většího počtu pixelů program udělá zadaný typ obrázku a zbytek je zaplněn černou barvou.
       - V případě špatně zadaného vstupu (nečíselný, nelze převést) u width a height > program vypíše chybovou hlášku.
       - V případě špatně zadaného vstupu u mode, program vypíše chybovou hlášku.
