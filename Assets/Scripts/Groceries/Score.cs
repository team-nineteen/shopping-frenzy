using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private const string spentMoneyTextFormat = "€{0},{1:D2}";
    private const string spentTimeTextFormat = "{0}:{1:D2}";
    const int BS = 1000; // Base score
    const int minimumTimeScore = BS / 10; // The lowest score you can get purely from a bad time.
    public enum Rank
    {
        S = BS, // At least the Base score. (really good time)
        A = (int)(BS * 0.9f), // at least 90%
        B = (int)(BS * 0.8f), // at least 80%
        C = (int)(BS * 0.6f), // ...
        D = (int)(BS * 0.4f),
        E = (int)(BS * 0.2f),
        F = 0,
        Z = -1 // Base case
    }

    private readonly Rank[] rankOrder = { Rank.S, Rank.A, Rank.B, Rank.C, Rank.D, Rank.E, Rank.F, Rank.Z };

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

    // score = BASE - (0.5 * cents_spent_too_much) - f(timeSpent, timeGoal)
    public void CalculateScore(int moneyGoal, int timeGoal)
    {
        // Every second late is deducted from score, every second early is added, arriving three times as late yields 0 score.
        this.score = (int)Mathf.Round((float)parabolaFromThreePoints(new Vector2(0, BS * 9f / 8f), new Vector2(timeGoal, BS), new Vector2(3 * timeGoal, 0), timeInSecondsSpent));
        this.score = Mathf.Max(this.score, minimumTimeScore);
        int moneyDiff = moneySpent - moneyGoal;
        // Every ct too much is .5pt deducted, every ct less is 5pt added
        this.score -= (int) (moneyDiff * (moneyDiff > 0 ? 0.5f : 5));
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

    private double parabolaFromThreePoints(Vector2 p1, Vector2 p2, Vector2 p3, int x)
    {
        double A1 = -p1.x * p1.x + p2.x * p2.x;
        double B1 = -p1.x + p2.x;
        double D1 = -p1.y + p2.y;
        double A2 = -p2.x * p2.x + p3.x * p3.x;
        double B2 = -p2.x + p3.x;
        double D2 = -p2.y + p3.y;
        double Bm = -(B2 / B1);
        double A3 = Bm * A1 + A2;
        double D3 = Bm * D1 + D2;
        double a = D3 / A3;
        double b = (D1 - A1 * a) / B1;
        double c = p1.y - a * p1.x * p1.x - b * p1.x;
        return a * x * x + b * x + c;
    }
}
