using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataSaver
{
    //it's static so we can call it from anywhere
    public static void SaveChessBoard(ChessPiece[,] chesspiece)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/savedGames.dat", FileMode.Create); //you can call it anything you want

        ChessData data = new ChessData(chesspiece);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static ChessData LoadChessBoard()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/savedGames.dat", FileMode.Open);

            ChessData data = bf.Deserialize(stream) as ChessData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("File does not exist!");
            return null;
        }
    }
}