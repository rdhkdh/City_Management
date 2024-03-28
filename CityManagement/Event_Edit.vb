﻿Imports System.Xml
Imports MySql.Data.MySqlClient
Imports Org.BouncyCastle.Asn1.IsisMtt.X509

Public Class Event_Edit

    ' Database connection string
    Dim connString As String = "server=172.16.114.244;userid=admin;Password=nimda;database=smart_city_management;sslmode=none"
    ' MySqlConnection object to handle communication with the MySQL database
    Dim conn As New MySqlConnection(connString)

    Dim EventId As Integer = 4
    Dim UserSID = 1

    ' This method is called when the Edit_Event form loads
    Private Sub Edit_Event_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the window state to maximized to make the form take up the full screen
        Me.WindowState = FormWindowState.Maximized

        ' Check if the global user ID from the login form is valid (not -1)
        If UserSID <> -1 Then
            ' Fetch event details from the database using the GetEventDetails function
            Dim eventDetails As Dictionary(Of String, Object) = GetEventDetails(EventId)
            If eventDetails IsNot Nothing Then
                ' If event details are found, populate the form fields with these details
                NameTextBox.Text = eventDetails("EventName").ToString()
                SpaceTextBox.Text = eventDetails("Venue").ToString()
                FeatureTextBox.Text = eventDetails("Restrictions").ToString()
                DescTextBox.Text = eventDetails("EventDescription").ToString()

                ' Attempt to parse the DateTime from the event details
                Dim eventDateTime As DateTime
                If DateTime.TryParse(eventDetails("DateTime").ToString(), eventDateTime) Then
                    DateTimePicker1.Value = eventDateTime
                    DateTimePicker2.Value = eventDateTime
                End If



                ' If a cover image is available, load it into the PictureBox
                Dim CoverImageData As Byte() = TryCast(eventDetails("CoverImage"), Byte())
                If CoverImageData IsNot Nothing Then
                    Dim ms As New System.IO.MemoryStream(CoverImageData)
                    PictureBox1.Image = Image.FromStream(ms)
                End If
            Else
                MessageBox.Show("Event details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("User not logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Retrieves festival details from the database using the provided EventID
    Private Function GetEventDetails(EventID As Integer) As Dictionary(Of String, Object)
        Dim festivalDetails As New Dictionary(Of String, Object)()
        Try
            conn.Open() ' Open database connection
            ' SQL query to fetch festival details
            Dim query As String = "SELECT name, description, image, owner_sid, vendor_service_tags, event_type, venue, dateTime, isapproved, isopen, restrictions FROM festivals WHERE id = @EventID"
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@EventID", EventID) ' Bind the EventID parameter
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    festivalDetails.Add("EventName", reader("name"))
                    festivalDetails.Add("DateTime", reader("dateTime").ToString())
                    festivalDetails.Add("Venue", reader("venue"))
                    festivalDetails.Add("EventDescription", reader("description"))
                    festivalDetails.Add("Restrictions", reader("restrictions"))
                    ' Assuming CoverImage is also being retrieved here based on your initial setup
                    festivalDetails.Add("CoverImage", If(Not IsDBNull(reader("image")), DirectCast(reader("image"), Byte()), Nothing))
                    ' Additional fields can be added as needed
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching festival details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close() ' Ensure the connection is closed even if an error occurs
        End Try
        Return festivalDetails
    End Function

    ' Event handler for the button click to upload a picture
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        With OpenFileDialog1
            .Title = "Select a picture" ' Title for the OpenFileDialog
            .Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG" ' Filter to only show image files
            .Multiselect = False ' Disallow selection of multiple files
        End With

        ' Show the OpenFileDialog and check the result
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                ' Load and display the selected image in the PictureBox
                Dim selectedImage As Image = Image.FromFile(OpenFileDialog1.FileName)
                PictureBox1.Image = selectedImage
            Catch ex As Exception
                ' Display an error message if loading the image fails
                MessageBox.Show("An error occurred while trying to load the picture: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' Event handler for the Save Changes button click
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            conn.Open() ' Open the database connection
            ' SQL query to update event details in the database
            Dim sql As String = "UPDATE festivals SET name = @EventName, venue = @Venue, restrictions = @Restrictions, description = @EventDescription, dateTime = @DateTime, event_type = @Category, image = @CoverImage WHERE id = @EventID"

            ' Combine Date and Time from the two DateTimePicker controls
            Dim combinedDateTime As DateTime = DateTimePicker1.Value.Date + DateTimePicker2.Value.TimeOfDay


            Using cmd As New MySqlCommand(sql, conn)
                ' Bind the form field values to the SQL query parameters
                cmd.Parameters.AddWithValue("@EventName", NameTextBox.Text)
                cmd.Parameters.AddWithValue("@Venue", SpaceTextBox.Text)
                cmd.Parameters.AddWithValue("@Restrictions", FeatureTextBox.Text)
                cmd.Parameters.AddWithValue("@EventDescription", DescTextBox.Text)
                cmd.Parameters.AddWithValue("@DateTime", combinedDateTime)

                ' Determine the category based on the radio button selection
                Dim category As String = ComboBox1.SelectedItem

                cmd.Parameters.AddWithValue("@Category", category)

                ' Convert the image in PictureBox to a byte array and bind it to the query parameter
                If PictureBox1.Image IsNot Nothing Then
                    Dim ms As New System.IO.MemoryStream()
                    PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                    Dim byteImage As Byte() = ms.ToArray()
                    cmd.Parameters.AddWithValue("@CoverImage", byteImage)
                Else
                    ' If no image is selected, bind a DBNull value
                    cmd.Parameters.AddWithValue("@CoverImage", DBNull.Value)
                End If

                ' Bind the EventID to the query parameter
                cmd.Parameters.AddWithValue("@EventID", EventID)

                ' Execute the SQL command
                cmd.ExecuteNonQuery()

                ' Display a success message
                MessageBox.Show("Event details updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            ' Display an error message if updating the event fails
            MessageBox.Show("Failed to update event details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close() ' Close the database connection to free resources
        End Try
    End Sub
End Class

