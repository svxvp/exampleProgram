using System;
using System.Collections.Generic;


namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            IState startState = new AuthState();
            while (startState != null) startState = startState.RunState();
        }
    }

    public interface IState
    {
        IState RunState();
    }

    public class MenuText
    {
        public string Text { get; set; }
    }
    public abstract class MenuItem : IState
    {   public string Value { get; set; }
        public long x { get; set; }
        public string i { get; set; }
        protected abstract Dictionary<int, MenuText> Menu { get; }

        protected virtual void ShowMenu()
        {
            
            foreach (var m in Menu)
                Console.WriteLine($"{m.Key} - {m.Value.Text}");
        }

        protected virtual KeyValuePair<int, MenuText> ReadOption()
        {
            Console.WriteLine("Выберите пункт меню");
            ShowMenu();

            var str = Console.ReadLine();

            if (int.TryParse(str, out int answerId))
            {
                if (!Menu.ContainsKey(answerId))
                {
                    Console.WriteLine("Ошибка выбора пункта меню");
                    return ReadOption();
                }
                return new KeyValuePair<int, MenuText>(answerId, Menu[answerId]);
            }
            else
            {
                Console.WriteLine("Ошибка выбора пункта меню");
                return ReadOption();
            }
        }
        public virtual IState RunState()
        {
            var option = ReadOption();
            return NextState(option);
        }

        protected abstract IState NextState(KeyValuePair<int, MenuText> selectedMenu);
    }


    public class MenuItems : MenuItem
    { 
        private Dictionary<int, MenuText> menu = new Dictionary<int, MenuText>() {
        { 1,new MenuText(){Text = "Из 2 в 10"}},
        { 2,new MenuText(){Text = "Из 10 в 2"}},
        { 3,new MenuText(){Text = "Из 10 в 16"}},
        {4, new MenuText(){Text = "Выход"}},
        {5, new MenuText(){Text = "Очистить экран"}}
        };
        protected override Dictionary<int, MenuText> Menu => menu;
        protected override IState NextState(KeyValuePair<int, MenuText> selectedMenu)
        {
            switch (selectedMenu.Key)
            {
                case 1:
                    return new MenuItem1().TranslateTo10();
                case 2:
                    return new MenuItem2().TranslateTo2();
                case 3:
                    return new MenuItem3().TranslateTo16();
                case 4:
                    return null;
                case 5:
                    return new MenuItem4().ClearCMD();
                default:
                    return this;
            }
             
        }

    }   

    public class MenuItem1:MenuItems
    {
        
        public IState TranslateTo10()
        {   
            Console.WriteLine("Перевод числа из 2 в 10");
            Console.WriteLine("Введите число в двоичной системе");
             Value = Console.ReadLine();
            try {
                x = Convert.ToInt32(Value, 2);
                Console.WriteLine("Ответ: " + x);
                Console.WriteLine("");
            }
            catch
            {
                Console.WriteLine("Ошибка ввода числа");
                Console.WriteLine("");
            }
            
            return this;
        }
        
    }

    public class MenuItem2:MenuItems
    {
        public IState TranslateTo2()
        {
            Console.WriteLine("Перевод числа из 10 в 2");
            Console.WriteLine("Введите число в десятичной системе");
            Value = Console.ReadLine();
            x = Convert.ToInt32(Value);
            i = Convert.ToString(x, 2);
            Console.WriteLine("Ответ: " + i);
            Console.WriteLine("");
            return this;
        }
    }

    public class MenuItem3 : MenuItems
    {
        public IState TranslateTo16()
        {
            Console.WriteLine("Перевод числа из 10 в 16");
            Console.WriteLine("Введите число в десятичной системе");
            Value = Console.ReadLine();
            x = Convert.ToInt32(Value);
            i = Convert.ToString(x, 16);
            Console.WriteLine("Ответ: " + i);
            Console.WriteLine("");
            return this;
        }
    }

    public class MenuItem4: MenuItems
    {
        public IState ClearCMD()
        {
            Console.Clear();
            return this;
        }
    }

    public class AuthState : IState
    {
        public IState RunState()
        {   MenuItems menu = new MenuItems();
            Console.WriteLine("Перевод чисел");
            return menu;
        }
        
    }
}
