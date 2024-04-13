﻿Imports MySql.Data.MySqlClient
Imports Windows.Media.Capture

Public Class specialisation
    Dim connectionString As String = "server=172.16.114.244;userid=admin;Password=nimda;database=smart_city_management;sslmode=none"

    Private Sub Guna2GradientTileButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientTileButton1.Click
    End Sub

    Private Sub Guna2GradientTileButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientTileButton2.Click
        Dim listHospitals As New listHospitals()

        ' Get the instance of MainForm (assuming MainForm is the parent form)
        Dim Temp2 As Temp2 = CType(Application.OpenForms("Temp2"), Temp2)

        ' Check if the main form instance is not null
        If Temp2 IsNot Nothing Then
            ' Call the public method of the main form to show the child form in the panel
            Temp2.ShowChildFormInPanel(listHospitals)
        End If
    End Sub

    Private Sub Guna2GradientTileButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientTileButton3.Click

    End Sub


    Private Sub Guna2GradientTileButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientTileButton4.Click
        Dim Health_ViewAppointment As New Health_ViewAppointment()

        ' Get the instance of MainForm (assuming MainForm is the parent form)
        Dim Temp2 As Temp2 = CType(Application.OpenForms("Temp2"), Temp2)

        ' Check if the main form instance is not null
        If Temp2 IsNot Nothing Then
            ' Call the public method of the main form to show the child form in the panel
            Temp2.ShowChildFormInPanel(Health_ViewAppointment)
        End If
    End Sub
    Private Sub Guna2PictureBox1_Click(sender As Object, e As EventArgs) Handles Guna2PictureBox1.Click

    End Sub


    Private Sub Panel1_Paint(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim queryString As String = "SELECT D.doctor_id, D.user_id, D.experience, D.rating, U.Name, U.Gender, H.name, H.location, H.contact " &
                            "FROM Doctors D " &
                            "INNER JOIN User U ON D.user_id = U.SID " &
                            "INNER JOIN hospitals H ON D.hos_id = H.hos_id "




        Using connection As New MySqlConnection(connectionString)
            Dim command As New MySqlCommand(queryString, connection)
            connection.Open()

            Dim reader As MySqlDataReader = command.ExecuteReader()
            Dim yPos As Integer = 0 ' Initial Y position for the first row
            Dim componentCount As Integer = 0 ' Counter for components in the current row

            Try
                While reader.Read()
                    ' Create an instance of cMakeAppointment with data from the database
                    Dim dName As String = reader("Name").ToString()
                    Dim hName As String = reader("name").ToString()
                    Dim experience As String = reader("experience").ToString()
                    Dim location As String = reader("location").ToString()
                    Dim contact As String = reader("contact").ToString()
                    Dim gender As String = reader("Gender").ToString()
                    Dim rating As String = reader("rating").ToString()


                    ' Create and position the cMakeAppointment component
                    Dim cspec As New cspecialisation(dName, hName, gender, experience, location, contact, rating)
                    cspec.Location = New Point(0, yPos) ' Set the location of the newHospital control
                    Panel1.Controls.Add(cspec) ' Add the newHospital control to Panel1

                    yPos += cspec.Height + 20 ' Update the Y position for the next row
                End While
            Finally
                reader.Close()
            End Try
        End Using


        'Dim specialist1 As New cspecialisation("Dr. John Doe", "abc Hospital", "Male", "10 Years", "Near IIG", "+91- 8749384923", 4.5)
        'Dim specialist2 As New cspecialisation("Dr. Jane Smith", "abc Hospital", "Male", "4 Years", "Near IIG", "+91- 8749384923", 3.8)
        'Dim specialist3 As New cspecialisation("Dr. Bob Johnson", "abc Hospital", "Male", "6 Years", "Near IIG ", "+91- 8749384923", 2.7)
        'Dim specialist4 As New cspecialisation("Dr. Hello", "abc Hospital", "Male", "5 Years", "Near IIG", "+91- 8749384923", 4.7)



        'Panel1.Controls.Add(specialist1)
        'Panel1.Controls.Add(specialist2)
        'Panel1.Controls.Add(specialist3)
        'Panel1.Controls.Add(specialist4)

        'If Panel1.Controls.Count > 1 Then
        '    Dim prevMessageLabel As Control = Panel1.Controls(Panel1.Controls.Count - 2)
        '    specialist1.Left = 20
        '    specialist2.Left = 20
        '    specialist3.Left = 20
        '    specialist4.Left = 20


        '    specialist1.Top = 0
        '    specialist2.Top = specialist1.Height + 20
        '    specialist3.Top = specialist1.Height + 20 + specialist2.Height + 20
        '    specialist4.Top = specialist1.Height + 20 + specialist2.Height + 20 + specialist3.Height + 20



    End Sub
End Class