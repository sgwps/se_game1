using System;

namespace Game_sem4{

    class PrintingItem{
        public char Symbol;
        public ConsoleColor Color;

        static List<char> BadChars = new List<char>() {'%', '@', '#', '_', '|'};


        public PrintingItem(char ParamSymabol = ' ', ConsoleColor ParamColor = ConsoleColor.Black){
            Symbol = ParamSymabol;
            Color = ParamColor;
        }


        public bool IsOccupied(){
            return BadChars.Contains(this.Symbol);
        }

        public void Print(){
            Console.ForegroundColor = Color;
            Console.Write(Symbol);
        }
    }

    class Rectangle{
        public const int Left = 9;
        public const int Right = 70;
        public const int Top = 5;
        public const int Bottom = 15;


        public static Position Center(){
            int VarX = (Right + Left) / 2;
            int VarY = (Top + Bottom) / 2;
            return new Position(VarX, VarY);
        }

        public static Position RightBottom(){
            return new Position(Right - 1, Bottom - 1);
        }

        public static Position RightBottomMinusOne(){
            return new Position(Right - 2, Bottom - 1);
        }

        public static Position RightBottomMinusTwo(){
            return new Position(Right - 3, Bottom - 1);
        }

    }

    class Checker{
        public const int MaxX = 79;  //start with 0
        public const int MaxY = 21;
        public static int CheckX(int NewX){
            if (NewX < 0 | NewX > MaxX) throw new ArgumentException();
            else return NewX;
        }


        public static int CheckY(int NewY){
            if (NewY < 0 | NewY > MaxY) throw new ArgumentException();
            else return NewY;
        }
    }


    class Position{
        
        private int VarX;
        private int VarY;

        public int X{
            get {return VarX;}
        }

        public int Y{
            get {return VarY;}
        }

        public Position (int NewX, int NewY){
            VarX = Checker.CheckX(NewX);
            VarY = Checker.CheckY(NewY);
        }

        private static int GetNewPosition(int Position, int Delta, int MinValue, int MaxValue){
            Position += Delta;
            if (Position <= MinValue) Position = MaxValue - (MinValue - Position + 1);
            if (Position >= MaxValue) Position = MinValue + (MaxValue - Position + 1);

            return Position;
        }


        private void SetX(int Delta){
            VarX = GetNewPosition(VarX, Delta, Rectangle.Left, Rectangle.Right);
        }


        private void SetY(int Delta){
            VarY = GetNewPosition(VarY, Delta, Rectangle.Top, Rectangle.Bottom);
        }



        public void MoveLeft(int Delta = 1){
            this.SetX(-Delta);
        }

        public void MoveRight(int Delta = 1){
            this.SetX(Delta);
        }

        public void MoveUp(int Delta = 1){
            this.SetY(-Delta);
        }

        public void MoveDown(int Delta = 1){
            this.SetY(Delta);
        }

        public Position Copy(){
            return new Position(VarX, VarY);
        }


        public Position MoveDownCopy(int Delta = 1){
            Position Copy = this.Copy();
            Copy.MoveDown(Delta);
            return Copy;
        }

        public Position MoveLeftCopy(int Delta = 1){
            Position Copy = this.Copy();
            Copy.MoveLeft(Delta);
            return Copy;
        }

        public Position MoveRightCopy(int Delta = 1){
            Position Copy = this.Copy();
            Copy.MoveRight(Delta);
            return Copy;
        }

        public Position MoveUpCopy(int Delta = 1){
            Position Copy = this.Copy();
            Copy.MoveUp(Delta);
            return Copy;
        }


        public double Distance(Position SecondPosition){
            double DifferenceX = Convert.ToDouble(Math.Abs(this.X - SecondPosition.X));
            double DifferenceY = Convert.ToDouble(Math.Abs(this.Y - SecondPosition.Y));
            double Result = DifferenceX * DifferenceX + DifferenceY * DifferenceY;
            return Math.Sqrt(Result);

        }


        public static Position RandomPosition(){
            Random rnd = new Random();
            return new Position(rnd.Next(Rectangle.Left + 1, Rectangle.Right), rnd.Next(Rectangle.Top + 1, Rectangle.Bottom));
        }




    }




    class Player{
        public Position Position;

        public Player(){
            Position = Rectangle.Center();
        }

        public void Move(ConsoleKeyInfo Key){
            
            if (Key.Key == ConsoleKey.RightArrow) Position.MoveRight();
            else if (Key.Key == ConsoleKey.LeftArrow)  Position.MoveLeft();
            else if (Key.Key == ConsoleKey.DownArrow) Position.MoveDown();
            else if (Key.Key == ConsoleKey.UpArrow) Position.MoveUp();
            if (!Field.IsFree(Position)) Program.Cyckle = false;
        }
    }


    class Stalcker{
        public Position Position;

        public Stalcker(){
            Position = Rectangle.RightBottom();
        }

        public void Move(Position PlayerPosition){
            List<Position> NewPositions = new List<Position>();
            Position PossiblePosition = this.Position.MoveDownCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            PossiblePosition = this.Position.MoveUpCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            PossiblePosition = this.Position.MoveLeftCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            PossiblePosition = this.Position.MoveRightCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            Position ClosestPosition = NewPositions[0];
            for(int i = 1; i < NewPositions.Count; i++)
            {
                if (ClosestPosition.Distance(PlayerPosition) > NewPositions[i].Distance(PlayerPosition)) ClosestPosition = NewPositions[i];
            }
            this.Position = ClosestPosition;
            Field.CheckPlayer(this.Position);
            Field.SetStalker(this);

        }
    }


    class Charger{
        public Position Position;
        public Position[] Positions = new Position[4];

        public Charger(){
            Position = Rectangle.RightBottomMinusOne();
            for (int i = 0; i < 4; i++) Positions[i] = Position;
        }

        public void Move(){
            bool PossiblityUp = true;
            bool PossiblityDown = true;
            bool PossiblityLeft = true;
            bool PossiblityRight = true;
            for (int i = 1; i <= 5; i++){
                Position PossiblePosition = this.Position.MoveUpCopy(i);
                if (!Field.IsFree(PossiblePosition)) PossiblityUp = false;
            }
            for (int i = 1; i <= 5; i++){
                Position PossiblePosition = this.Position.MoveDownCopy(i);
                if (!Field.IsFree(PossiblePosition)) PossiblityDown = false;
            }
            for (int i = 1; i <= 5; i++){
                Position PossiblePosition = this.Position.MoveLeftCopy(i);
                if (!Field.IsFree(PossiblePosition)) PossiblityLeft = false;
            }
            for (int i = 1; i <= 5; i++){
                Position PossiblePosition = this.Position.MoveRightCopy(i);
                if (!Field.IsFree(PossiblePosition)) PossiblityRight = false;
            }
            List<int> Ways = new List<int>();
            if (PossiblityUp) Ways.Add(0);
            if (PossiblityDown) Ways.Add(1);
            if (PossiblityLeft) Ways.Add(2);
            if (PossiblityRight) Ways.Add(3);
            Random rnd = new Random();
            int Way = Ways[rnd.Next(Ways.Count())];
            switch (Way){
                case 0:
                for (int i = 1; i <= 4; i++){
                    Positions[i - 1] = this.Position.MoveUpCopy(i);
                }
                Position.MoveUp(5);
                break;
                case 1:
                for (int i = 1; i <= 4; i++){
                    Positions[i - 1] = this.Position.MoveDownCopy(i);
                }
                Position.MoveDown(5);
                break;
                case 2:
                for (int i = 1; i <= 4; i++){
                    Positions[i - 1] = this.Position.MoveLeftCopy(i);
                }
                Position.MoveLeft(5);
                break;
                case 3:
                for (int i = 1; i <= 4; i++){
                    Positions[i - 1] = this.Position.MoveRightCopy(i);
                }
                Position.MoveRight(5);
                break;
            }
            Field.CheckPlayer(this.Position);
            Field.SetCharger(this);
        }
    }


    class Coin{
        public Position Position;
        public static int PlayerCoins = 0;
        public static int MonsterCoins = 0;

        public Coin(){
            Position = Position.RandomPosition();
        }

        private void SetNewPosition(){
            Position = Position.RandomPosition();
        }

        public void CheckPlayer(){

            if (Field.IsPlayer(Position)){
                PlayerCoins++;
                SetNewPosition();
            }
        }

        public void CheckMonster(){
            if (!Field.IsFree(Position)){
                MonsterCoins++;
                SetNewPosition();
            }
        }
    }


    class Wizzard{
        public Position Position;

        public Wizzard(){
            Position = Rectangle.RightBottomMinusTwo();
        }

        public void Move(){
            List<Position> NewPositions = new List<Position>();
            Position PossiblePosition = this.Position.MoveDownCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            PossiblePosition = this.Position.MoveUpCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            PossiblePosition = this.Position.MoveLeftCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            PossiblePosition = this.Position.MoveRightCopy();
            if (Field.IsFree(PossiblePosition)) NewPositions.Add(PossiblePosition);
            Random rnd = new Random();
            this.Position = NewPositions[rnd.Next(NewPositions.Count())];
            Field.CheckPlayer(this.Position);
            Field.SetWizzard(this);
        }
    }


    class Field{


        static int MaxX = 79;
        static int MaxY = 20;

        


        public static PrintingItem[][] CreateMap(){
            PrintingItem[][] MapLocal = new PrintingItem[MaxY + 1][];
            for (int i = 0; i <= MaxY; i++){
                MapLocal[i] = new PrintingItem[MaxX + 1];
                for (int j = 0; j <= MaxX; j++) MapLocal[i][j] = new PrintingItem();
            }
            return MapLocal;
        }

        static PrintingItem[][] Map = CreateMap();

        public static bool IsFree(Position ParamPosition){
            return !Map[ParamPosition.Y][ParamPosition.X].IsOccupied();
        }

        public static bool IsPlayer(Position ParamPosition){
            if (Map[ParamPosition.Y][ParamPosition.X].Symbol == '*') return true;
            else return false;
        }


        public static void CheckPlayer(Position ParamPosition){
            if (IsPlayer(ParamPosition)) Program.Cyckle = false;
        }

        static void SetPosition(int X, int Y, char Symbol, ConsoleColor Color){
            Map[Y][X] = new PrintingItem(Symbol, Color);
        }

        static void SetPosition(Position position, char Symbol, ConsoleColor Color){
            Map[position.Y][position.X] = new PrintingItem(Symbol, Color);
        }


        private static void SetRectangle(){
            for (int i = Rectangle.Left; i <= Rectangle.Right; i++) 
            {
                SetPosition(i, Rectangle.Top, '_', ConsoleColor.White);
                SetPosition(i, Rectangle.Bottom, '_', ConsoleColor.White);

            }
            
            for (int i = Rectangle.Top + 1; i <= Rectangle.Bottom - 1; i++) 
            {
                SetPosition(Rectangle.Left, i, '|', ConsoleColor.White);
                SetPosition(Rectangle.Right, i, '|', ConsoleColor.White);

            }
            

        }

        public static void SetPlayer(Player PlayerVar){
            SetPosition(PlayerVar.Position, '*', ConsoleColor.Black);
        }

        public static void SetWizzard(Wizzard WizzardVar){
            SetPosition(WizzardVar.Position, '#', ConsoleColor.Blue);
        }


        public static void SetCharger(Charger ChargerVar){
            for (int i = 0; i < 4; i++){
                SetPosition(ChargerVar.Positions[i], '%', ConsoleColor.Gray);
            }
            SetPosition(ChargerVar.Position, '%', ConsoleColor.Magenta);
        }


        public static void SetStalker(Stalcker StalckerVar){
            SetPosition(StalckerVar.Position, '@', ConsoleColor.Red);

        }

        public static void SetCoin(Coin CoinVar){
            SetPosition(CoinVar.Position, 'q', ConsoleColor.Red);

        }


        public static void DrawAll(Player PlayerVar, Wizzard WizzardVar, Charger ChargerVar, Stalcker StalckerVar, Coin CoinVar){
            SetEmpty();
            SetPlayer(PlayerVar);
            SetCharger(ChargerVar);
            SetWizzard(WizzardVar);
            SetStalker(StalckerVar);
            CoinVar.CheckMonster();
            CoinVar.CheckPlayer();


            SetCoin(CoinVar);
            Draw();
            Console.WriteLine($"{Coin.PlayerCoins}, {Coin.MonsterCoins}");
        }

        public static void SetEmpty(){
            Clean();
            SetRectangle();
        }

        

        public static void Draw(){
            foreach(PrintingItem[] i in Map){
                foreach(PrintingItem j in i){
                    j.Print();
                }
                Console.Write('\n');
            }
        }

        private static void Clean(){
            Map = CreateMap();
            SetRectangle();
        }
    }


    

    class Program{
        public static bool Cyckle = true;
        static void Main(){
            Console.BackgroundColor = ConsoleColor.Green;
            Player PlayerVar = new Player();
            Charger ChargerVar = new Charger();
            Wizzard WizzardVar = new Wizzard();
            Stalcker StalckerVar = new Stalcker();
            Coin CoinVar = new Coin();
            
            while (Cyckle){
                Field.DrawAll(PlayerVar, WizzardVar, ChargerVar, StalckerVar, CoinVar);
                ConsoleKeyInfo Key = Console.ReadKey();
                PlayerVar.Move(Key);
                Field.DrawAll(PlayerVar, WizzardVar, ChargerVar, StalckerVar, CoinVar);
                Field.SetEmpty();
                Field.SetPlayer(PlayerVar);
                ChargerVar.Move();
                WizzardVar.Move();
                StalckerVar.Move(PlayerVar.Position);
                if (Key.Key == ConsoleKey.Escape) Cyckle = false;
            }
            Field.SetEmpty();
            Field.Draw();
            Console.ReadKey();
        }
    }
}