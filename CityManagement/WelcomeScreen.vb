﻿
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

Public Class WelcomeScreen
    ' Make this form full screen
    Private Sub WelcomeScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Label1.BackColor = System.Drawing.Color.Transparent
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MainPanel.Show()
        Me.Hide()
    End Sub
End Class