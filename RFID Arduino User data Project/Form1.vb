Imports MySql.Data.MySqlClient
Imports System
Imports System.IO.Ports


Public Class Form1

    Dim Connection As New MySqlConnection("server=localhost; user=root; password= ; database=arduino_project")
    Dim MySQLCMD As New MySqlCommand
    Dim MySQLDA As New MySqlDataAdapter
    Dim DT As New DataTable
    Dim Table_Name As String = "rfid_user_data" 'your table name
    Dim intime_Table_Name As String = "intime"
    Dim outtime_Table_Name As String = "outtime"
    Dim Data As Integer

    Dim LoadImagesStr As Boolean = False
    Dim IDRam As String
    Dim IMG_FileNameInput As String
    Dim StatusInput As String = "Save"
    Dim SqlCmdSearchstr As String

    Public Shared StrSerialIn As String
    Dim GetID As Boolean = False
    Dim ViewUserData As Boolean = False
    Dim comPort As String


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        PanelConnection.Visible = True
        PanelUserData.Visible = False
        PanelRegistrationandEditUserData.Visible = False
        PanelInTime.Visible = False
        PanelOutTime.Visible = False
        ComboBoxBaudRate.SelectedIndex = 3

    End Sub

    Private Sub ShowData()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            If LoadImagesStr = False Then
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "SELECT  Sr_No,Name, Father_Name, Roll_No,Student_ID, Address,Phone_No,Date,Time FROM " & Table_Name & " ORDER BY Name"
                MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
                DT = New DataTable
                Data = MySQLDA.Fill(DT)
                If Data > 0 Then
                    DataGridView1.DataSource = Nothing
                    DataGridView1.DataSource = DT
                    DataGridView1.Columns(2).DefaultCellStyle.Format = "c"
                    DataGridView1.DefaultCellStyle.ForeColor = Color.Black
                    DataGridView1.ClearSelection()
                Else
                    DataGridView1.DataSource = DT
                End If
            Else
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "SELECT image FROM " & Table_Name & " WHERE Student_ID LIKE '" & IDRam & "'"
                MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
                DT = New DataTable
                Data = MySQLDA.Fill(DT)
                If Data > 0 Then
                    Dim ImgArray() As Byte = DT.Rows(0).Item("image")
                    Dim lmgStr As New System.IO.MemoryStream(ImgArray)
                    PictureBoxImagePreview.Image = Image.FromStream(lmgStr)
                    PictureBoxImagePreview.SizeMode = PictureBoxSizeMode.Zoom
                    lmgStr.Close()
                End If
                LoadImagesStr = False
            End If
        Catch ex As Exception
            MsgBox("Failed to load Database !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()
    End Sub
    Private Sub ShowDataInTime()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            If LoadImagesStr = False Then
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "SELECT * FROM " & intime_Table_Name & " ORDER BY Sr_No"
                MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
                DT = New DataTable
                Data = MySQLDA.Fill(DT)
                If Data > 0 Then
                    DataGridView2.DataSource = Nothing
                    DataGridView2.DataSource = DT
                    DataGridView2.Columns(2).DefaultCellStyle.Format = "c"
                    DataGridView2.DefaultCellStyle.ForeColor = Color.Black
                    DataGridView2.ClearSelection()
                Else
                    DataGridView2.DataSource = DT
                End If
            End If
        Catch ex As Exception
            MsgBox("Failed to load Database !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()
    End Sub
    Private Sub ShowDataOutTime()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            If LoadImagesStr = False Then
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "SELECT * FROM " & outtime_Table_Name & " ORDER BY Sr_No"
                MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
                DT = New DataTable
                Data = MySQLDA.Fill(DT)
                If Data > 0 Then
                    DataGridView3.DataSource = Nothing
                    DataGridView3.DataSource = DT
                    DataGridView3.Columns(2).DefaultCellStyle.Format = "c"
                    DataGridView3.DefaultCellStyle.ForeColor = Color.Black
                    DataGridView3.ClearSelection()
                Else
                    DataGridView3.DataSource = DT
                End If
            End If
        Catch ex As Exception
            MsgBox("Failed to load Database !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()
    End Sub

    Private Sub ShowDataUser()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLCMD.CommandType = CommandType.Text
            MySQLCMD.CommandText = "SELECT * FROM " & Table_Name & " WHERE Student_ID LIKE '" & LabelID.Text.Substring(5, LabelID.Text.Length - 5) & "'"
            MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                Dim ImgArray() As Byte = DT.Rows(0).Item("image")
                Dim lmgStr As New System.IO.MemoryStream(ImgArray)
                PictureBoxUserImage.Image = Image.FromStream(lmgStr)
                lmgStr.Close()

                'LabelID.Text = "ID : " & DT.Rows(0).Item("student_ID")
                LabelName.Text = DT.Rows(0).Item("Name")
                LabelFatherName.Text = DT.Rows(0).Item("Father_Name")
                LabelRollNo.Text = DT.Rows(0).Item("Roll_No")
                LabelID.Text = DT.Rows(0).Item("student_ID")
                LabelAdress.Text = DT.Rows(0).Item("Address")
                LabelPhoneNo.Text = DT.Rows(0).Item("Phone_No")
                If RadioButtonIn.Checked = True Then
                    Try
                        MySQLCMD = New MySqlCommand
                        With MySQLCMD
                            .CommandText = "INSERT INTO intime ( Name, Father_Name, Roll_No,Student_ID, Address, Phone_No) VALUES (@name, @fathername, @rollno,@studentid,  @address,@phoneno)"
                            .Connection = Connection
                            .Parameters.AddWithValue("@name", LabelName.Text)
                            .Parameters.AddWithValue("@fathername", LabelFatherName.Text)
                            .Parameters.AddWithValue("@rollno", LabelRollNo.Text)
                            .Parameters.AddWithValue("@studentid", LabelID.Text)
                            .Parameters.AddWithValue("@address", LabelAdress.Text)
                            .Parameters.AddWithValue("@phoneno", LabelPhoneNo.Text)
                            .ExecuteNonQuery()



                        End With
                        SerialPort1.Write("1")
                        ClearInputUpdateData()
                    Catch ex As Exception
                        MsgBox("Data failed to save !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
                        Connection.Close()
                        Return
                    End Try

                ElseIf RadioButtonOut.Checked = True Then
                    Try
                        MySQLCMD = New MySqlCommand
                        With MySQLCMD
                            .CommandText = "INSERT INTO outtime ( Name, Father_Name, Roll_No,Student_ID, Address, Phone_No) VALUES ( @name, @fathername, @rollno,@studentid, @address,@phoneno)"
                            .Connection = Connection
                            .Parameters.AddWithValue("@name", LabelName.Text)
                            .Parameters.AddWithValue("@fathername", LabelFatherName.Text)
                            .Parameters.AddWithValue("@rollno", LabelRollNo.Text)
                            .Parameters.AddWithValue("@studentid", LabelID.Text)
                            .Parameters.AddWithValue("@address", LabelAdress.Text)
                            .Parameters.AddWithValue("@phoneno", LabelPhoneNo.Text)
                            .ExecuteNonQuery()
                        End With
                        SerialPort1.Write("2")
                        ClearInputUpdateData()
                    Catch ex As Exception
                        MsgBox("Data failed to save !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
                        Connection.Close()
                        Return
                    End Try
                End If
            Else
                    MsgBox("ID not found !!!" & vbCr & "Please register your ID.", MsgBoxStyle.Information, "Information Message")
            End If
        Catch ex As Exception
            MsgBox("Failed to load Database !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()
    End Sub

    Private Sub ClearInputUpdateData()
        TextBoxName.Text = ""
        LabelGetID.Text = "________"
        TextBoxFatherName.Text = ""
        TextBoxRollNo.Text = ""
        TextBoxAdress.Text = ""
        TextBoxPhoneNo.Text = ""
        PictureBoxImageInput.Image = My.Resources.Click_to_browse
    End Sub

    Private Sub ButtonConnection_Click(sender As Object, e As EventArgs) Handles ButtonConnection.Click
        PictureBoxSelect.Top = ButtonConnection.Top
        PanelUserData.Visible = False
        PanelRegistrationandEditUserData.Visible = False
        PanelConnection.Visible = True
        PanelInTime.Visible = False
        PanelOutTime.Visible = False
    End Sub

    Private Sub ButtonUserData_Click(sender As Object, e As EventArgs) Handles ButtonUserData.Click
        If TimerSerialIn.Enabled = False Then
            MsgBox("Failed to open User Data !!!" & vbCr & "Click the Connection menu then click the Connect button.", MsgBoxStyle.Information, "Information")
            Return
        Else
            StrSerialIn = ""
            ViewUserData = True
            PictureBoxSelect.Top = ButtonUserData.Top
            PanelRegistrationandEditUserData.Visible = False
            PanelConnection.Visible = False
            PanelUserData.Visible = True
            PanelInTime.Visible = False
            PanelOutTime.Visible = False
        End If
    End Sub

    Private Sub ButtonRegistration_Click(sender As Object, e As EventArgs) Handles ButtonRegistration.Click
        StrSerialIn = ""
        ViewUserData = False
        PictureBoxSelect.Top = ButtonRegistration.Top
        PanelConnection.Visible = False
        PanelUserData.Visible = False
        PanelRegistrationandEditUserData.Visible = True
        PanelInTime.Visible = False
        PanelOutTime.Visible = False
        ShowData()
    End Sub

    Private Sub PanelConnection_Paint(sender As Object, e As PaintEventArgs) Handles PanelConnection.Paint
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2), PanelConnection.ClientRectangle)
    End Sub

    Private Sub PanelConnection_Resize(sender As Object, e As EventArgs) Handles PanelConnection.Resize
        PanelConnection.Invalidate()
    End Sub

    Private Sub PanelUserData_Paint(sender As Object, e As PaintEventArgs) Handles PanelUserData.Paint
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2), PanelUserData.ClientRectangle)
    End Sub

    Private Sub PanelUserData_Resize(sender As Object, e As EventArgs) Handles PanelUserData.Resize
        PanelUserData.Invalidate()
    End Sub

    Private Sub PanelRegistrationandEditUserData_Paint(sender As Object, e As PaintEventArgs) Handles PanelRegistrationandEditUserData.Paint
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2), PanelRegistrationandEditUserData.ClientRectangle)
    End Sub

    Private Sub PanelRegistrationandEditUserData_Resize(sender As Object, e As EventArgs) Handles PanelRegistrationandEditUserData.Resize
        PanelRegistrationandEditUserData.Invalidate()
    End Sub

    Private Sub ButtonScanPort_Click(sender As Object, e As EventArgs) Handles ButtonScanPort.Click
        ComboBoxPort.Items.Clear()
        Dim myPort As Array
        Dim i As Integer
        myPort = IO.Ports.SerialPort.GetPortNames()
        ComboBoxPort.Items.AddRange(myPort)
        i = ComboBoxPort.Items.Count
        i = i - i
        Try
            ComboBoxPort.SelectedIndex = i
        Catch ex As Exception
            MsgBox("Com port not detected", MsgBoxStyle.Critical, "Error Message")
            ComboBoxPort.Text = ""
            ComboBoxPort.Items.Clear()
            Return
        End Try
        ComboBoxPort.DroppedDown = True
    End Sub

    Private Sub ButtonScanPort_MouseHover(sender As Object, e As EventArgs) Handles ButtonScanPort.MouseHover
        ButtonScanPort.ForeColor = Color.White
    End Sub

    Private Sub ButtonScanPort_MouseLeave(sender As Object, e As EventArgs) Handles ButtonScanPort.MouseLeave
        ButtonScanPort.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonConnect_Click(sender As Object, e As EventArgs) Handles ButtonConnect.Click
        If ButtonConnect.Text = "Connect" Then
            SerialPort1.BaudRate = ComboBoxBaudRate.SelectedItem
            SerialPort1.PortName = ComboBoxPort.SelectedItem
            Try
                SerialPort1.Open()
                TimerSerialIn.Start()
                ButtonConnect.Text = "Disconnect"
                PictureBoxStatusConnect.Image = My.Resources.Connected
            Catch ex As Exception
                MsgBox("Failed to connect !!!" & vbCr & "Arduino is not detected.", MsgBoxStyle.Critical, "Error Message")
                PictureBoxStatusConnect.Image = My.Resources.Disconnect
            End Try
        ElseIf ButtonConnect.Text = "Disconnect" Then
            PictureBoxStatusConnect.Image = My.Resources.Disconnect
            ButtonConnect.Text = "Connect"
            LabelConectionStatus.Text = "Connection Status : Disconnect"
            TimerSerialIn.Stop()
            SerialPort1.Close()
        End If
    End Sub
    Private Sub ButtonConnect_MouseHover(sender As Object, e As EventArgs) Handles ButtonConnect.MouseHover
        ButtonConnect.ForeColor = Color.White
    End Sub

    Private Sub ButtonConnect_MouseLeave(sender As Object, e As EventArgs) Handles ButtonConnect.MouseLeave
        ButtonConnect.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        LabelID.Text = "ID : ________"
        LabelName.Text = "Waiting..."
        LabelFatherName.Text = "Waiting..."
        LabelRollNo.Text = "Waiting..."
        LabelAdress.Text = "Waiting..."
        LabelPhoneNo.Text = "Waiting..."
        PictureBoxUserImage.Image = Nothing
    End Sub
    Private Sub ButtonClear_MouseHover(sender As Object, e As EventArgs) Handles ButtonClear.MouseHover
        ButtonClear.ForeColor = Color.White
    End Sub

    Private Sub ButtonClear_MouseLeave(sender As Object, e As EventArgs) Handles ButtonClear.MouseLeave
        ButtonClear.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        Dim mstream As New System.IO.MemoryStream()
        Dim arrImage() As Byte

        If TextBoxName.Text = "" Then
            MessageBox.Show("Name cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If TextBoxFatherName.Text = "" Then
            MessageBox.Show("Father Name cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If TextBoxRollNo.Text = "" Then
            MessageBox.Show("Roll No cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If TextBoxAdress.Text = "" Then
            MessageBox.Show("Address cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If TextBoxPhoneNo.Text = "" Then
            MessageBox.Show("Phone No cannot be empty !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If StatusInput = "Save" Then
            If IMG_FileNameInput <> "" Then
                PictureBoxImageInput.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
                arrImage = mstream.GetBuffer()
            Else
                MessageBox.Show("The image has not been selected !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Try
                Connection.Open()
            Catch ex As Exception
                MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            Try
                MySQLCMD = New MySqlCommand
                With MySQLCMD
                    .CommandText = "INSERT INTO " & Table_Name & " ( Name, Father_Name, Roll_No,Student_ID, Address, Phone_No, image) VALUES ( @name, @fathername, @rollno,@studentid, @address,@phoneno, @image)"
                    .Connection = Connection
                    .Parameters.AddWithValue("@name", TextBoxName.Text)
                    .Parameters.AddWithValue("@fathername", TextBoxFatherName.Text)
                    .Parameters.AddWithValue("@rollno", TextBoxRollNo.Text)
                    .Parameters.AddWithValue("@studentid", LabelGetID.Text)
                    .Parameters.AddWithValue("@address", TextBoxAdress.Text)
                    .Parameters.AddWithValue("@phoneno", TextBoxPhoneNo.Text)
                    .Parameters.AddWithValue("@image", arrImage)
                    .ExecuteNonQuery()
                End With
                MsgBox("Data saved successfully", MsgBoxStyle.Information, "Information")
                IMG_FileNameInput = ""
                ClearInputUpdateData()
            Catch ex As Exception
                MsgBox("Data failed to save !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
                Connection.Close()
                Return
            End Try
            Connection.Close()

        Else

            If IMG_FileNameInput <> "" Then
                PictureBoxImageInput.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
                arrImage = mstream.GetBuffer()

                Try
                    Connection.Open()
                Catch ex As Exception
                    MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                Try
                    MySQLCMD = New MySqlCommand
                    With MySQLCMD
                        .CommandText = "UPDATE " & Table_Name & " SET  Name=@name,Father_Name=@fathername,Roll_No=@rollno,Student_ID=@studentid,Address=@address,Phone_No=@phoneno,image=@image WHERE Student_ID=@studentid "
                        .Connection = Connection
                        .Parameters.AddWithValue("@name", TextBoxName.Text)
                        .Parameters.AddWithValue("@fathername", TextBoxFatherName.Text)
                        .Parameters.AddWithValue("@rollno", TextBoxRollNo.Text)
                        .Parameters.AddWithValue("@studentid", LabelGetID.Text)
                        .Parameters.AddWithValue("@address", TextBoxAdress.Text)
                        .Parameters.AddWithValue("@phoneno", LabelPhoneNo.Text)
                        .Parameters.AddWithValue("@image", arrImage)
                        .ExecuteNonQuery()
                    End With
                    MsgBox("Data updated successfully", MsgBoxStyle.Information, "Information")
                    IMG_FileNameInput = ""
                    ButtonSave.Text = "Save"
                    ClearInputUpdateData()
                Catch ex As Exception
                    MsgBox("Data failed to Update !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
                    Connection.Close()
                    Return
                End Try
                Connection.Close()

            Else

                Try
                    Connection.Open()
                Catch ex As Exception
                    MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End Try

                Try
                    MySQLCMD = New MySqlCommand
                    With MySQLCMD
                        .CommandText = "UPDATE " & Table_Name & " SET Name=@name,Father_Name=@fathername,Roll_No=@rollno,Student_ID=@studentid,Address=@address,Phone_No=@phoneno WHERE Student_ID=@studentid "
                        .Connection = Connection
                        .Parameters.AddWithValue("@name", TextBoxName.Text)
                        .Parameters.AddWithValue("@fathername", TextBoxFatherName.Text)
                        .Parameters.AddWithValue("@rollno", TextBoxRollNo.Text)
                        .Parameters.AddWithValue("@studentid", LabelGetID.Text)
                        .Parameters.AddWithValue("@address", TextBoxAdress.Text)
                        .Parameters.AddWithValue("@phoneno", LabelPhoneNo.Text)
                        .ExecuteNonQuery()
                    End With
                    MsgBox("Data updated successfully", MsgBoxStyle.Information, "Information")
                    ButtonSave.Text = "Save"
                    ClearInputUpdateData()
                Catch ex As Exception
                    MsgBox("Data failed to Update !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
                    Connection.Close()
                    Return
                End Try
                Connection.Close()
            End If
            StatusInput = "Save"
        End If
        PictureBoxImagePreview.Image = Nothing
        ShowData()
    End Sub

    Private Sub ButtonSave_MouseHover(sender As Object, e As EventArgs) Handles ButtonSave.MouseHover
        ButtonSave.ForeColor = Color.White
    End Sub

    Private Sub ButtonSave_MouseLeave(sender As Object, e As EventArgs) Handles ButtonSave.MouseLeave
        ButtonSave.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonClearForm_Click(sender As Object, e As EventArgs) Handles ButtonClearForm.Click
        ClearInputUpdateData()
    End Sub


    Private Sub ButtonClearForm_MouseHover(sender As Object, e As EventArgs) Handles ButtonClearForm.MouseHover
        ButtonClearForm.ForeColor = Color.White
    End Sub

    Private Sub ButtonClearForm_MouseLeave(sender As Object, e As EventArgs) Handles ButtonClearForm.MouseLeave
        ButtonClearForm.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub ButtonScanID_Click(sender As Object, e As EventArgs) Handles ButtonScanID.Click
        If TimerSerialIn.Enabled = True Then
            PanelReadingTagProcess.Visible = True
            GetID = True
            ButtonScanID.Enabled = False
        Else
            MsgBox("Failed to open User Data !!!" & vbCr & "Click the Connection menu then click the Connect button.", MsgBoxStyle.Critical, "Error Message")
        End If
    End Sub

    Private Sub ButtonScanID_MouseHover(sender As Object, e As EventArgs) Handles ButtonScanID.MouseHover
        ButtonScanID.ForeColor = Color.White
    End Sub

    Private Sub ButtonScanID_MouseLeave(sender As Object, e As EventArgs) Handles ButtonScanID.MouseLeave
        ButtonScanID.ForeColor = Color.FromArgb(6, 71, 165)
    End Sub

    Private Sub PictureBoxImageInput_Click(sender As Object, e As EventArgs) Handles PictureBoxImageInput.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "JPEG (*.jpeg;*.jpg)|*.jpeg;*.jpg"

        If (OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            IMG_FileNameInput = OpenFileDialog1.FileName
            PictureBoxImageInput.ImageLocation = IMG_FileNameInput
        End If
    End Sub

    Private Sub CheckBoxByName_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxByName.CheckedChanged
        If CheckBoxByName.Checked = True Then
            CheckBoxByID.Checked = False
        End If
        If CheckBoxByName.Checked = False Then
            CheckBoxByID.Checked = True
        End If
    End Sub

    Private Sub CheckBoxByID_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxByID.CheckedChanged
        If CheckBoxByID.Checked = True Then
            CheckBoxByName.Checked = False
        End If
        If CheckBoxByID.Checked = False Then
            CheckBoxByName.Checked = True
        End If
    End Sub

    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        If CheckBoxByID.Checked = True Then
            If TextBoxSearch.Text = Nothing Then
                SqlCmdSearchstr = "SELECT Sr_No, Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & Table_Name & " ORDER BY Name"
            Else
                SqlCmdSearchstr = "SELECT Sr_No, Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & Table_Name & " WHERE Student_ID LIKE'" & TextBoxSearch.Text & "%'"
            End If
        End If
        If CheckBoxByName.Checked = True Then
            If TextBoxSearch.Text = Nothing Then
                SqlCmdSearchstr = "SELECT Sr_No, Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & Table_Name & " ORDER BY Name"
            Else
                SqlCmdSearchstr = "SELECT Sr_No, Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & Table_Name & " WHERE Name LIKE'" & TextBoxSearch.Text & "%'"
            End If
        End If

        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLDA = New MySqlDataAdapter(SqlCmdSearchstr, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                DataGridView1.DataSource = Nothing
                DataGridView1.DataSource = DT
                DataGridView1.DefaultCellStyle.ForeColor = Color.Black
                DataGridView1.ClearSelection()
            Else
                DataGridView1.DataSource = DT
            End If
        Catch ex As Exception
            MsgBox("Failed to search" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
        End Try
        Connection.Close()
    End Sub
    Private Sub TextBoxSearchInTime_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearchInTime.TextChanged
        If CheckBoxByIDInTime.Checked = True Then
            If TextBoxSearchInTime.Text = Nothing Then
                SqlCmdSearchstr = "SELECT  * FROM " & intime_Table_Name & " ORDER BY Sr_No"
            Else
                SqlCmdSearchstr = "SELECT * FROM " & intime_Table_Name & " WHERE Roll_No LIKE'" & TextBoxSearchInTime.Text & "%'"
            End If
        End If
        If CheckBoxByNameInTime.Checked = True Then
            If TextBoxSearchInTime.Text = Nothing Then
                SqlCmdSearchstr = "SELECT * FROM " & intime_Table_Name & " ORDER BY Sr_No"
            Else
                SqlCmdSearchstr = "SELECT * FROM " & intime_Table_Name & " WHERE Name LIKE'" & TextBoxSearchInTime.Text & "%'"
            End If
        End If

        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLDA = New MySqlDataAdapter(SqlCmdSearchstr, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                DataGridView2.DataSource = Nothing
                DataGridView2.DataSource = DT
                DataGridView2.DefaultCellStyle.ForeColor = Color.Black
                DataGridView2.ClearSelection()
            Else
                DataGridView2.DataSource = DT
            End If
        Catch ex As Exception
            MsgBox("Failed to search" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
        End Try
        Connection.Close()
    End Sub
    Private Sub TextBoxSearchOutTime_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearchOutTime.TextChanged
        If CheckBoxByIDInTime.Checked = True Then
            If TextBoxSearchInTime.Text = Nothing Then
                SqlCmdSearchstr = "SELECT  Sr_No,Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & outtime_Table_Name & " ORDER BY Sr_No"
            Else
                SqlCmdSearchstr = "SELECT Sr_No,Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & outtime_Table_Name & " WHERE Roll_No LIKE'" & TextBoxSearchOutTime.Text & "%'"
            End If
        End If
        If CheckBoxNameOutTime.Checked = True Then
            If TextBoxSearchOutTime.Text = Nothing Then
                SqlCmdSearchstr = "SELECT Sr_No, Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & outtime_Table_Name & " ORDER BY Sr_No"
            Else
                SqlCmdSearchstr = "SELECT Sr_No, Name, Father_Name, Roll_No,Student_ID, Address,Phone_No FROM " & outtime_Table_Name & " WHERE Name LIKE'" & TextBoxSearchOutTime.Text & "%'"
            End If
        End If

        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLDA = New MySqlDataAdapter(SqlCmdSearchstr, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                DataGridView3.DataSource = Nothing
                DataGridView3.DataSource = DT
                DataGridView3.DefaultCellStyle.ForeColor = Color.Black
                DataGridView3.ClearSelection()
            Else
                DataGridView3.DataSource = DT
            End If
        Catch ex As Exception
            MsgBox("Failed to search" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
        End Try
        Connection.Close()
    End Sub

    Private Sub DataGridView1_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown
        Try
            If AllCellsSelected(DataGridView1) = False Then
                If e.Button = MouseButtons.Left Then
                    DataGridView1.CurrentCell = DataGridView1(e.ColumnIndex, e.RowIndex)
                    Dim i As Integer
                    With DataGridView1
                        If e.RowIndex >= 0 Then
                            i = .CurrentRow.Index
                            LoadImagesStr = True
                            IDRam = .Rows(i).Cells("Student_ID").Value.ToString
                            ShowData()
                        End If
                    End With
                End If
            End If
        Catch ex As Exception
            Return
        End Try
    End Sub
    Private Sub DataGridView2_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView2.CellMouseDown
        Try
            If AllCellsSelected(DataGridView2) = False Then
                If e.Button = MouseButtons.Left Then
                    DataGridView2.CurrentCell = DataGridView2(e.ColumnIndex, e.RowIndex)
                    Dim i As Integer
                    With DataGridView2
                        If e.RowIndex >= 0 Then
                            i = .CurrentRow.Index
                            LoadImagesStr = True
                            IDRam = .Rows(i).Cells("Student_ID").Value.ToString
                            ShowDataInTime()
                        End If
                    End With
                End If
            End If
        Catch ex As Exception
            Return
        End Try
    End Sub
    Private Sub DataGridView3_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView3.CellMouseDown
        Try
            If AllCellsSelected(DataGridView3) = False Then
                If e.Button = MouseButtons.Left Then
                    DataGridView3.CurrentCell = DataGridView3(e.ColumnIndex, e.RowIndex)
                    Dim i As Integer
                    With DataGridView3
                        If e.RowIndex >= 0 Then
                            i = .CurrentRow.Index
                            LoadImagesStr = True
                            IDRam = .Rows(i).Cells("Student_ID").Value.ToString
                            ShowDataOutTime()
                        End If
                    End With
                End If
            End If
        Catch ex As Exception
            Return
        End Try
    End Sub

    Private Function AllCellsSelected(dgv As DataGridView) As Boolean
        AllCellsSelected = (DataGridView1.SelectedCells.Count = (DataGridView1.RowCount * DataGridView1.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
    End Function
    Private Function AllCellsSelected2(dgv As DataGridView) As Boolean
        AllCellsSelected2 = (DataGridView2.SelectedCells.Count = (DataGridView2.RowCount * DataGridView2.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
    End Function
    Private Function AllCellsSelected3(dgv As DataGridView) As Boolean
        AllCellsSelected3 = (DataGridView3.SelectedCells.Count = (DataGridView3.RowCount * DataGridView3.Columns.GetColumnCount(DataGridViewElementStates.Visible)))
    End Function

    Private Sub TimerTimeDate_Tick(sender As Object, e As EventArgs) Handles TimerTimeDate.Tick
        LabelDateTime.Text = "Time " & DateTime.Now.ToString("HH:mm:ss") & "  Date " & DateTime.Now.ToString("dd MMM, yyyy")
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If DataGridView1.RowCount = 0 Then
            MsgBox("Cannot delete, table data is empty", MsgBoxStyle.Critical, "Error Message")
            Return
        End If


        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Cannot delete, select the table data to be deleted", MsgBoxStyle.Critical, "Error Message")
            Return
        End If
        If MsgBox("Delete record?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Confirmation") = MsgBoxResult.Cancel Then Return

        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            If AllCellsSelected(DataGridView1) = True Then
                MySQLCMD.CommandType = CommandType.Text
                MySQLCMD.CommandText = "DELETE FROM " & Table_Name
                MySQLCMD.Connection = Connection
                MySQLCMD.ExecuteNonQuery()
            End If
            For Each row As DataGridViewRow In DataGridView1.SelectedRows
                If row.Selected = True Then
                    MySQLCMD.CommandType = CommandType.Text
                    MySQLCMD.CommandText = "DELETE FROM " & Table_Name & " WHERE Student_ID='" & row.DataBoundItem(4).ToString & "'"
                    MySQLCMD.Connection = Connection
                    MySQLCMD.ExecuteNonQuery()
                End If
            Next
        Catch ex As Exception
            MsgBox("Failed to delete" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
        End Try
        PictureBoxImagePreview.Image = Nothing
        Connection.Close()
        ShowData()

    End Sub


    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        DataGridView1.SelectAll()
        DataGridView2.SelectAll()
        DataGridView3.SelectAll()
    End Sub

    Private Sub ClearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearToolStripMenuItem.Click
        DataGridView1.ClearSelection()
        DataGridView2.ClearSelection()
        DataGridView3.ClearSelection()
        PictureBoxImagePreview.Image = Nothing
    End Sub

    Private Sub RefreshToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem1.Click
        ShowData()
        ShowDataInTime()
        ShowDataOutTime()
    End Sub

    Private Sub TimerSerialIn_Tick(sender As Object, e As EventArgs) Handles TimerSerialIn.Tick
        Try
            StrSerialIn = SerialPort1.ReadExisting
            LabelConectionStatus.Text = "Connection Status : Connected"
            If StrSerialIn <> "" Then
                If GetID = True Then
                    LabelGetID.Text = StrSerialIn
                    GetID = False
                    If LabelGetID.Text <> "________" Then
                        PanelReadingTagProcess.Visible = False
                        IDCheck()
                    End If
                End If
                If ViewUserData = True Then
                    ViewData()
                End If
            End If
        Catch ex As Exception
            TimerSerialIn.Stop()
            SerialPort1.Close()
            LabelConectionStatus.Text = "Connection Status : Disconnect"
            PictureBoxStatusConnect.Image = My.Resources.Disconnect
            MsgBox("Failed to connect !!!" & vbCr & "Arduino is not detected.", MsgBoxStyle.Critical, "Error Message")
            ButtonConnect_Click(sender, e)
            Return
        End Try

        If PictureBoxStatusConnect.Visible = True Then
            PictureBoxStatusConnect.Visible = False
        ElseIf PictureBoxStatusConnect.Visible = False Then
            PictureBoxStatusConnect.Visible = True
        End If
    End Sub

    Private Sub IDCheck()
        Try
            Connection.Open()
        Catch ex As Exception
            MessageBox.Show("Connection failed !!!" & vbCrLf & "Please check that the server is ready !!!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Try
            MySQLCMD.CommandType = CommandType.Text
            MySQLCMD.CommandText = "SELECT * FROM " & Table_Name & " WHERE Student_ID LIKE '" & LabelGetID.Text & "'"
            MySQLDA = New MySqlDataAdapter(MySQLCMD.CommandText, Connection)
            DT = New DataTable
            Data = MySQLDA.Fill(DT)
            If Data > 0 Then
                If MsgBox("ID registered !" & vbCr & "Do you want to edit the data ?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Confirmation") = MsgBoxResult.Cancel Then
                    DT = Nothing
                    Connection.Close()
                    ButtonScanID.Enabled = True
                    GetID = False
                    LabelGetID.Text = "________"
                    Return
                Else
                    Dim ImgArray() As Byte = DT.Rows(0).Item("image")
                    Dim lmgStr As New System.IO.MemoryStream(ImgArray)
                    PictureBoxImageInput.Image = Image.FromStream(lmgStr)
                    PictureBoxImageInput.SizeMode = PictureBoxSizeMode.Zoom

                    TextBoxName.Text = DT.Rows(0).Item("Name")
                    TextBoxFatherName.Text = DT.Rows(0).Item("Father_Name")
                    TextBoxRollNo.Text = DT.Rows(0).Item("Roll_No")
                    TextBoxAdress.Text = DT.Rows(0).Item("Address")
                    TextBoxPhoneNo.Text = DT.Rows(0).Item("Phone_No")
                    StatusInput = "Update"
                End If
            End If
        Catch ex As Exception
            MsgBox("Failed to load Database !!!" & vbCr & ex.Message, MsgBoxStyle.Critical, "Error Message")
            Connection.Close()
            Return
        End Try

        DT = Nothing
        Connection.Close()

        ButtonScanID.Enabled = True
        GetID = False
    End Sub

    Private Sub ViewData()
        LabelID.Text = "ID : " & StrSerialIn
        If LabelID.Text = "ID : ________" Then
            ViewData()
        Else
            ShowDataUser()
        End If
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        GroupBoxImage.Location = New Point((PanelUserData.Width / 2) - (GroupBoxImage.Width / 2), GroupBoxImage.Top)
        PanelReadingTagProcess.Location = New Point((PanelRegistrationandEditUserData.Width / 2) - (PanelReadingTagProcess.Width / 2), 106)
    End Sub

    Private Sub ButtonCloseReadingTag_Click(sender As Object, e As EventArgs) Handles ButtonCloseReadingTag.Click
        PanelReadingTagProcess.Visible = False
        ButtonScanID.Enabled = True
    End Sub

    Private Sub ButtonInTime_Click(sender As Object, e As EventArgs) Handles ButtonInTime.Click
        StrSerialIn = ""
        ViewUserData = False
        PictureBoxSelect.Top = ButtonInTime.Top
        PanelConnection.Visible = False
        PanelUserData.Visible = False
        PanelRegistrationandEditUserData.Visible = False
        PanelInTime.Visible = True
        PanelOutTime.Visible = False
        ShowDataInTime()
    End Sub

    Private Sub ButtonOutTime_Click(sender As Object, e As EventArgs) Handles ButtonOutTime.Click
        StrSerialIn = ""
        ViewUserData = False
        PictureBoxSelect.Top = ButtonOutTime.Top
        PanelConnection.Visible = False
        PanelUserData.Visible = False
        PanelRegistrationandEditUserData.Visible = False
        PanelInTime.Visible = False
        PanelOutTime.Visible = True

        ShowDataOutTime()
    End Sub

End Class