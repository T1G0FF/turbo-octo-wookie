using System.Linq;
using System.Collections.Generic;
using System;

namespace Assignment1
{
    class CrozzleCreation
    {
        private struct Direction { public static bool Horizontal = true; public static bool Vertical = false; }

        private Wordlist currentWordlist;
        private List<string> WordsByScoreAll;
        private List<string> WordsByScoreCurrent;
        private List<Crozzle> GeneratedList;
        private CrozzleArray currentCrozzle;

        private string currWordSegement = "";
        private string prevWord = "";
        private int wordIndex   = -1;
        private int prevRow     = 0;
        private int currRow     = 0;
        private int prevColumn  = 0;
        private int currColumn  = 0;

        public int TotalWords   = 0;

        public CrozzleCreation(Wordlist wordlist)
        {
            currentWordlist = wordlist;
            currentCrozzle = new CrozzleArray(wordlist.Height, wordlist.Width);
            GeneratedList = new List<Crozzle>();

            ScoredWordlist tempScoredList = new ScoredWordlist(wordlist);
            WordsByScoreAll = new List<string>(wordlist.WordCount);
            WordsByScoreCurrent = new List<string>(wordlist.WordCount);
            foreach (CrozzleWord cw in tempScoredList)
            {
                WordsByScoreAll.Add(cw.Word);
                WordsByScoreCurrent.Add(cw.Word);
            }
        }

        public bool GetBestCrozzle()
        {
            CrozzleArray currentCrozzle = getCrozzle();
            CheckAndAdd(currentCrozzle);
            return GeneratedList.Count > 1;
        }

        public List<Crozzle> Finalise()
        {
            return GeneratedList;
        }

        private bool CheckAndAdd(CrozzleArray crozzleArray)
        {
            bool added = false;
            if (crozzleArray != null)
            {
                CrozzleArray tempCA = new CrozzleArray(crozzleArray.Height, crozzleArray.Width);
                tempCA.DeepCopy(crozzleArray);
                Crozzle croz = new Crozzle(tempCA._crozzleGrid);
                GeneratedList.Add(croz);
                added = true;
            }
            return added;
        }
        
        private CrozzleArray getCrozzle()
        {
            bool end = false;
            WordsByScoreCurrent = null;
            WordsByScoreCurrent = new List<string>();
            foreach (String ss in WordsByScoreAll)
            {
                WordsByScoreCurrent.Add(ss);
            }

            wordIndex++;

            if (wordIndex != WordsByScoreCurrent.Count || wordIndex + 1 < WordsByScoreAll.Count)
            {
                currentCrozzle.FillWithBlanks();

                prevRow = 0;
                currRow = 0;
                prevColumn = 0;
                currColumn = 0;

                PlaceFirstWord(WordsByScoreCurrent[wordIndex]);                
#if DEBUG
                displayStatus();
#endif

                if (TryPlacement(prevWord, 1, 0, 0, Direction.Vertical))
                {
                    CheckAndAdd(currentCrozzle);
#if DEBUG
                    displayStatus();
#endif
                    if (currWordSegement.Length <= 1)
                    {
                        end = true;
                    }
                    else
                    {
                        currWordSegement = currWordSegement.Remove(0, 1);

                        if (currColumn == prevColumn)
                        {
                            if (!TryPlacement(currWordSegement, 1, currRow, currColumn + 2, Direction.Horizontal))
                            {
                                end = true;
                            }
                        }
                        else
                        {
                            
                            if (!TryPlacement(currWordSegement, currColumn - prevColumn - 1, currRow, currColumn + 1, Direction.Horizontal))
                            {
                                end = true;
                            }
                        }
                    }
                }
                else
                {
                    end = true;
                }
                
#if DEBUG
                displayStatus();
#endif
                if (!end)
                {
                    if (currWordSegement.Length <= 1)
                    {
                        end = true;
                    }
                    else
                    {
                        currWordSegement = currWordSegement.Remove(0, 1);

                        while (TryPlacement(currWordSegement, currRow - prevRow - 1, currRow + 2, currColumn, Direction.Vertical))
                        {
                            CheckAndAdd(currentCrozzle);
#if DEBUG
                            displayStatus();                            
#endif
                            if (currWordSegement.Length <= 1)
                            {
                                end = true;
                            }
                            else
                            {
                                currWordSegement = currWordSegement.Remove(0, 1);

                                if (!TryPlacement(currWordSegement, currColumn - currColumn - 1, currRow, currColumn + 2, Direction.Horizontal))
                                {
                                    end = true;
                                }
                                else
                                {   
#if DEBUG
                                    displayStatus();
#endif
                                    if (currWordSegement.Length <= 1)
                                    {
                                        end = true;
                                    }
                                    currWordSegement = currWordSegement.Remove(0, 1);
                                }
                                CheckAndAdd(currentCrozzle);
                                if (end) break;
                            }
                        }
                        end = true;
                    }
                }
            }

            if (end)
            {
                CheckAndAdd(currentCrozzle);
                return getCrozzle();
            }
            else
            {
                return currentCrozzle;
            }
        }

        #region Placement
        private void PlaceFirstWord(string word)
        {
            currWordSegement = word;
            prevWord = word;
            PlaceWord(word, 0, 0, Direction.Horizontal);
        }

        private void PlaceWord(string word, int row, int column, bool horizontal)
        {
            int WordLength = word.Length;
            int WordEnd = WordLength - 1;

            if (horizontal)
            {
                for (int i = 0; i < WordLength; i++)
                {
                    currentCrozzle[row + i, column] = word[i];
                }
                prevColumn = currColumn;
                currColumn = column;
                
            }
            else
            {
                for (int i = 0; i < WordLength; i++)
                {
                    currentCrozzle[row, column + i] = word[i];
                }
                prevRow = currRow;
                currRow = row;
            }

            prevWord = word;
            WordsByScoreCurrent.Remove(word);
        }
        #endregion
        
        #region Try to Place
        private bool TryPlacement(string currentWord, int gap, int column, int row, bool horizontal)
        {
            bool result = false;

            try
            {
                if (horizontal)
                {
                    string wordSegment = "";
                    foreach (string word in WordsByScoreCurrent)
                    {
                        wordSegment = word.Substring(0, gap);

                        for (int i = 0; i < wordSegment.Length; i++)
                        {
                            for (int j = 0; j < currentWord.Length; j++)
                            {
                                if (currentWord[j] == wordSegment[i])
                                {
                                    if (WillFit(word, column - i, row + j, Direction.Horizontal))
                                    {
                                        PlaceWord(word, column - i, row + j, Direction.Horizontal);
                                        TotalWords++;
                                        result = true;
                                    }
                                }
                                if (result) break;
                            }
                            if (result) break;
                        }
                        if (result) break;
                    }
                }
                else
                {
                    string wordSegment = "";
                    foreach (string word in WordsByScoreCurrent)
                    {
                        wordSegment = word.Substring(0, gap);

                        for (int i = 0; i < wordSegment.Length; i++)
                        {
                            for (int j = 0; j < currentWord.Length; j++)
                            {
                                if (currentWord[j] == wordSegment[i])
                                {
                                    if (WillFit(word, column + j, row - i, Direction.Vertical))
                                    {
                                        PlaceWord(word, column + j, row - i, Direction.Vertical);
                                        TotalWords++;
                                        result = true;
                                    }
                                }
                                if (result) break;
                            }
                            if (result) break;
                        }
                        if (result) break;
                    }
                }
            }
            catch
            {
                CheckAndAdd(currentCrozzle);
                result = false;
            }
            return result;
        }
        #endregion
        
        #region Fit
        private bool WillFit(string word, int column, int row, bool horizontal)
        {
            bool result = true;
            int WordLength = word.Length;
            int WordEnd = WordLength - 1;
            int Tries = 2;

            try
            {
                if (horizontal)
                {
                    if (column + WordLength > currentCrozzle.Width)
                    {
                        result = false;
                    }
                    else
                    {
                        for (int i = 0; i < WordLength; i++)
                        {
                            if (currentCrozzle[column + i, row] != ' ')
                            {
                                if (word[i] != currentCrozzle[column + i, row])
                                {
                                    result = false;
                                    break;
                                }
                                else
                                {
                                    Tries--;
                                    currWordSegement = word.Substring(i + 1, WordEnd - i);
                                    if (Tries == 0)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (row + WordLength > currentCrozzle.Height)
                    {
                        result = false;
                    }
                    else
                    {
                        for (int i = 0; i < WordLength; i++)
                        {
                            if (currentCrozzle[column, row + i] != ' ')
                            {
                                if (word[i] != currentCrozzle[column, row + i])
                                {
                                    result = false;
                                    break;
                                }
                                else
                                {
                                    Tries--;
                                    currWordSegement = word.Substring(i + 1, WordEnd - i);
                                    if (Tries == 0)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
                result = false;
            }
            
            return result;
        }
        #endregion

#if DEBUG
        public void displayStatus()
        {
            displayStatus(currentCrozzle);
        }

        public void displayStatus(CrozzleArray crozz)
        {
            string line = new string('=', 60);
            LogFile.WriteLine(line);
            for (int y = 0; y < crozz.Height; y++)
            {
                for (int x = 0; x < crozz.Width; x++)
                {
                    if (crozz[x, y] == ' ')
                    {
                        LogFile.Write("-" + "|");
                    }
                    else
                    {
                        LogFile.Write(crozz[x, y] + "|");
                    }
                }
                LogFile.WriteLine("");
            }
            LogFile.WriteLine(line);
        }
#endif
    }
}
