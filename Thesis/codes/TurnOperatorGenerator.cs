void GeneratePlayerOperators()
{
    // Összes karakter összes operátorát tartalmazó lista
    List<List<PlayerAction>> actions = new List<List<PlayerAction>>();
    for (int i = 0; i < Options.friendlyCount; i++)
    {
        // Minden karakterre legeneráljuk az összes interakciót
        // Itt még nem operátorról beszélünk
        List<PlayerAction> playerActionI = new List<PlayerAction>
        {
            new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.UP),
            new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.DOWN),
            new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.LEFT),
            new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.RIGHT),
            new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.UP),
            new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.DOWN),
            new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.LEFT),
            new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.RIGHT)
        };
        // A karakterhez tartozó lehetséges lépéseket egy listába gyüjtjük, amit szintén egy listába 
        actions.Add(playerActionI);
    }


    // Egyszerűség kedvéért kigyújtük az egyes listák hosszát
    List<int> lenghts = actions.Select(x => x.Count).ToList();
    // Mivel nem fix karakterszámmal játszódik a játék, ezért kell egy dinamikusan generálandó indexlista
    List<int> indexes = actions.Select(x => 0).ToList();
    // Akkor végzünk a szorzással, amikor az első listán végig értünk
    while (indexes[0] < lenghts[0])
    {
        // A lista, amit majd az operátor tartalmazni fog
        List<PlayerAction> playerActions = new List<PlayerAction>();
        for (int i = 0; i < actions.Count; i++)
        {
            // Minden al-listából kivesszük azt az elemet, ahol a lista indexénél tartunk
            playerActions.Add(actions[i][indexes[i]]);
        }
        // Létrehozzuk az új operátort
        Operators.Add(new TurnOperator(playerActions));
        // Lépünk a következő párosításra
        for (int i = indexes.Count - 1; i >= 0; i--)
        {
            // Először az utolsó lista indexét növeljük meg
            indexes[i]++;
            // Ha még nem értünk végig a listán, vagy már az első listán vagyunk, akkor a következő iterációra lépünk
            if (indexes[i] < lenghts[i] || i == 0) break;
            // Ellenkező esetben a lista végére értünk, ezért vissza állunk a lista elejére az indexszel. 
            // A ciklus a következő iterációban megnöveli az előtte lévő lista indexét eggyel 
            // (illetve ha annak a végére értünk, akkor azt is nullára állítja, és növeli az előtte lévőt, ezt folytatva az első listáig), ezzel biztosítva, hogy minden lehetséges kombináció szisztematikusan létrehozásra kerül
            indexes[i] = 0;
        }
    }
}
