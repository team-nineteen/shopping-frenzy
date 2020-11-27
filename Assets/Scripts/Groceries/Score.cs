using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private const string spentMoneyTextFormat = "€{0},{1:D2}";
    private const string spentTimeTextFormat = "{0}:{1:D2}";
    const int BS = 1000; // Base score
    public enum Rank
    {
        S = BS, // At least the Base score. (really good time)
        A = (int)(BS * 0.9f), // at least 90%
        B = (int)(BS * 0.8f), // at least 80%
        C = (int)(BS * 0.7f), // ...
        D = (int)(BS * 0.6f),
        E = (int)(BS * 0.5f),
        F = (int)(BS * 0.4f),
        Z = -1 // Base case
    }

    private readonly Rank[] rankOrder = {Rank.S, Rank.A, Rank.B, Rank.C, Rank.D, Rank.E, Rank.F, Rank.Z};

    public int moneySpent { get; set; } = 0;
    public int timeInSecondsSpent { get; set; } = 0;
    public int score { get; private set; } = 0;
    public Rank rank { get; private set; } = Rank.Z;

    // A score keeps track of money and time spent and evaluates a rank based on how good that score is.
    private Rank FindHighestRank()
    {
        foreach (Rank r in rankOrder)
        {
            if (this.score >= (int)r) return r;
        }
        return Rank.Z;
    }

    public void CalculateScore(int moneyGoal, int timeGoal)
    {
        this.score = BS; // Base score, deduct points based on how bad you did :)
        if (moneySpent > moneyGoal)
        {
            this.score -= (moneySpent - moneyGoal) / 10; // Every 10 cents spent too much is deducted from the score.
        }
        this.score -= (timeInSecondsSpent - timeGoal); // Every second late is deducted from score, every second early is added.

        this.rank = FindHighestRank();
    }

    public static string MoneyString(int cents)
    {
        return string.Format(spentMoneyTextFormat, cents / 100, cents % 100);
    }
    public static string TimeString(int seconds)
    {
        return string.Format(spentTimeTextFormat, seconds / 60, seconds % 60);
    }
}
