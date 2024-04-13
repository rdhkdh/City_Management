﻿Imports System.Globalization
Imports System.Windows.Documents
Imports MySql.Data.MySqlClient
Imports Org.BouncyCastle.Asn1.Cms

Public Class makeAppointment
    Dim connectionString As String = "server=172.16.114.244;userid=admin;Password=nimda;database=smart_city_management;sslmode=none"
    Dim date1 As Date
    Shared hos_id As String
    Shared stimee As String
    Shared uid As String
    Dim spec As String = ""
    Dim datee As String
    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub makeAppointment_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2PictureBox2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        spec = ComboBox2.SelectedItem
    End Sub

    Private Sub Guna2TextBox2_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2GradientTileButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientTileButton1.Click
        Dim specialisation As New specialisation()

        ' Get the instance of MainForm (assuming MainForm is the parent form)
        Dim Temp2 As Temp2 = CType(Application.OpenForms("Temp2"), Temp2)

        ' Check if the main form instance is not null
        If Temp2 IsNot Nothing Then
            ' Call the public method of the main form to show the child form in the panel
            Temp2.ShowChildFormInPanel(specialisation)
        End If
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

    Public Sub Guna2DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles Guna2DateTimePicker1.ValueChanged
        date1 = Guna2DateTimePicker1.Value
        datee = date1.ToString("yyyy-mm-dd")
    End Sub

    Private Sub Guna2TextBox2_TextChanged_1(sender As Object, e As EventArgs)

    End Sub
    Shared Sub makeappointment(id As String)
        hos_id = id


    End Sub

    Shared hid As String, did As String
    Public Sub senddata(h_id As String, d_id As String, t As String, u As String)
        hid = h_id
        did = d_id
        stimee = t
        uid = u
    End Sub


    Private Sub Panel1_Paint(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Query the Hospital table to fetch all rows
        Dim queryString As String = "SELECT D.doctor_id, D.user_id, D.experience, D.rating, D.visiting_time_start, D.visiting_time_end, U.Name, U.Gender " &
                                    "FROM Doctors D " &
                                    "INNER JOIN User U ON D.user_id = U.SID " &
                                    "WHERE D.hos_id ='" & hos_id & "';"

        Using connection As New MySqlConnection(connectionString)
            Dim command As New MySqlCommand(queryString, connection)
            connection.Open()

            Dim reader As MySqlDataReader = command.ExecuteReader()
            Dim yPos As Integer = 0 ' Initial Y position for the first row
            Dim componentCount As Integer = 0 ' Counter for components in the current row

            Try
                While reader.Read()
                    ' Create an instance of cMakeAppointment with data from the database
                    Dim Name As String = reader("Name").ToString()
                    Dim experience As String = reader("experience").ToString()
                    Dim gender As String = reader("Gender").ToString()
                    Dim stime As String = reader("visiting_time_start").ToString()
                    Dim etime As String = reader("visiting_time_end").ToString()

                    Dim d_id As String = reader("doctor_id").ToString()
                    Dim uid As String = reader("user_id").ToString()
                    Dim rating As String = reader("rating").ToString()

                    ' Create and position the cMakeAppointment component
                    Dim cmake As New cMakeAppointment(Name, experience, gender, stime + " to " + etime, rating, hos_id, d_id, stime, uid)
                    cmake.Location = New Point(componentCount * (cmake.Width + 10), yPos)
                    Panel1.Controls.Add(cmake)

                    ' Increment component count for the current row
                    componentCount += 1

                    ' If 4 components are reached, start a new row
                    If componentCount >= 4 Then
                        yPos += cmake.Height + 10 ' Adjust Y position for the next row
                        componentCount = 0 ' Reset component count for the new row
                    End If
                End While
            Finally
                reader.Close()
            End Try
        End Using


    End Sub

    Private Sub Guna2TileButton1_Click(sender As Object, e As EventArgs) Handles Guna2TileButton1.Click
        Using connection As New MySqlConnection(connectionString)
            Dim queryString As String = "INSERT INTO hospitalAppointment ( hos_id, doctor_id, user_id, appointmentNo, status, date, time) " &
                                "VALUES ( @hos_id, @doctor_id, @user_id, @appointmentNo, @status, @date, @time)"
            Dim command As New MySqlCommand(queryString, connection)

            command.Parameters.AddWithValue("@doctor_id", did)
            command.Parameters.AddWithValue("@hos_id", hos_id)
            command.Parameters.AddWithValue("@user_id", uid)
            command.Parameters.AddWithValue("@appointmentNo", 9)
            command.Parameters.AddWithValue("@status", 0)
            command.Parameters.AddWithValue("@date", datee)
            command.Parameters.AddWithValue("@time", stimee)
            connection.Open()
            Dim affectedRows As Integer = command.ExecuteNonQuery()
            If affectedRows > 0 Then
                ' Insert successful
                MessageBox.Show("Appointment is made")
            Else
                ' Insert failed
            End If
        End Using

    End Sub

    Public Sub Guna2Button4_Click_1(sender As Object, e As EventArgs) Handles Guna2Button4.Click


        Using connection As New MySqlConnection(connectionString)

            Dim queryString As String
            If (spec = "") Then
                queryString = "SELECT D.doctor_id, D.user_id, D.experience, D.rating, D.visiting_time_start, D.visiting_time_end, U.Name, U.Gender, " &
                            "(SELECT COUNT(*) FROM hospitalAppointment HA " &
                            "WHERE HA.doctor_id = D.doctor_id AND HA.date = @datee) AS count " &
                            "FROM Doctors D " &
                            "INNER JOIN User U ON D.user_id = U.SID " &
                            "WHERE D.hos_id = @hos_id"


            Else
                queryString = "SELECT D.doctor_id, D.user_id, D.experience, D.rating, D.visiting_time_start, D.visiting_time_end, U.Name, U.Gender, " &
                            "(SELECT COUNT(*) FROM hospitalAppointment HA " &
                            "WHERE HA.doctor_id = D.doctor_id AND HA.date = @datee) AS count " &
                            "FROM Doctors D " &
                            "INNER JOIN User U ON D.user_id = U.SID " &
                            "WHERE D.hos_id = @hos_id AND D.speciality = @spec;"

            End If

            Dim command As New MySqlCommand(queryString, connection)
            ' Assuming you have parameters for datee, hos_id, and spec
            ' You can add them as parameters to your MySqlCommand
            command.Parameters.AddWithValue("@datee", datee)
            command.Parameters.AddWithValue("@hos_id", hos_id)
            command.Parameters.AddWithValue("@spec", spec)

            connection.Open()

            Dim reader As MySqlDataReader = command.ExecuteReader()
            Dim yPos As Integer = 0 ' Initial Y position for the first row
            Dim componentCount As Integer = 0 ' Counter for components in the current row
            Panel1.Controls.Clear()
            Try
                While reader.Read()
                    ' Create an instance of cMakeAppointment with data from the database
                    Dim Name As String = reader("Name").ToString()
                    Dim experience As String = reader("experience").ToString()
                    Dim gender As String = reader("Gender").ToString()
                    Dim stime As String = reader("visiting_time_start").ToString()
                    Dim etime As String = reader("visiting_time_end").ToString()
                    Dim count As String = reader("count").ToString()

                    Dim timeString As String = stime
                    Dim parsedTime As TimeSpan
                    TimeSpan.TryParseExact(timeString, "hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, parsedTime)
                    Dim hour As Int32 = parsedTime.Hours
                    Dim min As Int32 = parsedTime.Minutes

                    timeString = etime
                    TimeSpan.TryParseExact(timeString, "hh\:mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, parsedTime)
                    Dim hour1 As Int32 = parsedTime.Hours
                    Dim min1 As Int32 = parsedTime.Minutes


                    hour = (hour * 60 + (count) * 20) / 60
                    min = (hour * 60 + (count) * 20) Mod 60

                    Dim d_id As String = reader("doctor_id").ToString()
                    Dim uid As String = reader("user_id").ToString()
                    Dim rating As String = reader("rating").ToString()

                    ' Create and position the cMakeAppointment component
                    Dim cmake As New cMakeAppointment(Name, experience, gender, "" + hour.ToString() + ":" + min.ToString() + " to " + hour1.ToString() + ":" + min1.ToString(), Double.Parse(rating), hos_id, d_id, stime, uid)
                    cmake.Location = New Point(componentCount * (cmake.Width + 20), yPos)
                    Panel1.Controls.Add(cmake)

                    ' Increment component count for the current row
                    componentCount += 1

                    ' If 4 components are reached, start a new row
                    If componentCount >= 3 Then
                        yPos += cmake.Height + 10 ' Adjust Y position for the next row
                        componentCount = 0 ' Reset component count for the new row
                    End If

                End While
            Finally
                reader.Close()
            End Try

        End Using
    End Sub
End Class