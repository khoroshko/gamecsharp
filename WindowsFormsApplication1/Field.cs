using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Field
    {
        public enum FieldValue
        {
            e = 0,
            O = 1,
            X = 2
        }
        private const int FIELD_SIZE = 3;
        private FieldValue[,] _cells;
        private int _stepIndex = 1;

        private int _FieldSize;
        public int FieldSize
        {
            get
            {
                return _FieldSize;
            }
            set
            {
                _FieldSize = value;
                _cells = new FieldValue[_FieldSize, _FieldSize];
                FillArray();
            }
        }

        public Field() :
            this(FIELD_SIZE)
        {
        }

        public Field(int fieldSize)
        {
            FieldSize = fieldSize;            
        }

        public FieldValue GetCellValue(Cell c)
        {
            return _cells[c.I, c.J];
        }

        public char this[Cell c]
        {
            get
            {
                char chRet = ' ';

                switch (_cells[c.I, c.J])
                {
                    case FieldValue.O:
                        chRet = '0';
                        break;
                    case FieldValue.X:
                        chRet = 'X';
                        break;
                    case FieldValue.e:
                    default:
                        chRet = ' ';
                        break;
                }

                return chRet;
            }
        }

        public void FillArray()
        {
            _stepIndex = 1;
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    _cells[i, j] = FieldValue.e;
                }
            }
        }

        public bool ChecksFilledCells()   //проверка что в массиве есть пустые ячейки
        {
            bool result = false;
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    if (_cells[i, j] == FieldValue.e)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        
        public Cell GetComputerChoiseAverageLevel()  //компьютер ходит случайны способом
        {
            Random rand = new Random();
            int choice = rand.Next(1, FieldSize * FieldSize + 2 - _stepIndex);
            Cell result = new Cell();
            bool b = false;
            int amount = 0;
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    if (_cells[i, j] == FieldValue.e)
                    {
                        amount++;
                        if (amount == choice)
                        {
                            result.I = i;
                            result.J = j;
                            b = true;
                            break;
                        }
                    }
                }
                if (b == true)
                {
                    break;
                }
            }
            return result;
        }

        public Cell GetCh(FieldValue v, int i)  
        {
            Cell f = GetChoice(v);
            if (f != null)
            {
                return f;
            }
            f = GetChoice(ChancheFieldValue(v));
            if (f != null)
            {
                return f;
            }
            return GetComputerChoiseAverageLevel();
        }

        public Cell GetChoice(FieldValue v)   //проверка для хода компьютера можно ли выиграть или помешать 
                                              //выиграть сопернику
        {
            bool result = false;
            int hor = 0;
            int ver = 0;
            int mdig = 0;
            int supdig = 0;
            Cell fHor = null;
            Cell fVer = null;
            Cell fMdig = null;
            Cell fSupdig = null;
            Cell fResult = null;

            for (int i = 0; i < FieldSize; i++)
            {
                hor = 0;
                ver = 0;
                fHor = null;
                fVer = null;
                for (int j = 0; j < FieldSize; j++)
                {
                    if (_cells[i, j] == v)
                    {
                        hor++;
                    }
                    else
                    {
                        if (_cells[i, j] == FieldValue.e)
                        {
                            fHor = new Cell(i, j);
                        }
                    }
                    if (_cells[j, i] == v)
                    {
                        ver++;
                    }
                    else
                    {
                        if (_cells[j, i] == FieldValue.e)
                        {
                            fVer = new Cell(j, i);
                        }
                    }
                }

                if (hor == FieldSize - 1 && fHor != null)
                {
                    fResult = fHor;
                    result = true;
                    break;
                }

                if (ver == FieldSize - 1 && fVer != null)
                {
                    fResult = fVer;
                    result = true;
                    break;
                }

                if (_cells[i, i] == v)
                {
                    mdig++;
                }
                else
                {
                    if (_cells[i, i] == FieldValue.e)
                    {
                        fMdig = new Cell(i, i);
                    }
                }
                if (_cells[i, FieldSize - 1 - i] == v)
                {
                    supdig++;
                }
                else
                {
                    if (_cells[i, FieldSize - 1 - i] == FieldValue.e)
                    {
                        fSupdig = new Cell(i, FieldSize - 1 - i);
                    }
                }
            }

            if (!result)
            {
                if (mdig == FieldSize - 1 && fMdig != null)
                {
                    fResult = fMdig;
                }
                else if (supdig == FieldSize - 1 && fSupdig != null)
                {
                    fResult = fSupdig;
                }
            }
            return fResult;
        }

        public bool AreAllCellsFilldIn(FieldValue v)    //проверка на выиграш
        {
            bool result = false;
            int hor = 0;
            int ver = 0;
            int mdig = 0;
            int supdig = 0;

            for (int i = 0; i < FieldSize; i++)
            {
                hor = 0;
                ver = 0;
                for (int j = 0; j < FieldSize; j++)
                {
                    if (_cells[i, j] == v)
                    {
                        hor++;
                    }
                    if (_cells[j, i] == v)
                    {
                        ver++;
                    }
                }
                if (hor == FieldSize || ver == FieldSize)
                {
                    result = true;
                }
            }
            mdig = 0;
            supdig = 0;
            for (int i = 0; i < FieldSize; i++)
            {
                if (_cells[i, i] == v)
                {
                    mdig++;
                }
                if (_cells[i, FieldSize - 1 - i] == v)
                {
                    supdig++;
                }
            }
            if (mdig == FieldSize || supdig == FieldSize)
            {
                result = true;
            }
            return result;
        }

        public FieldValue ChancheFieldValue(FieldValue f)   //меняется текущая фигура
        {
            return f == FieldValue.X ? FieldValue.O : FieldValue.X;
        }

        public Cell GetCellByComputer(FieldValue v)  //возвращает ячейку для хода компьютера
        {
            return GetCh(v, _stepIndex);
        }

        public void DoStep(Cell c, FieldValue f) //соперник делает ход
        {
            ++_stepIndex;
            _cells[c.I, c.J] = f;
        }
    }
}
