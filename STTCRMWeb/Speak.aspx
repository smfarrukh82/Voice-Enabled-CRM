<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Speak.aspx.cs" Inherits="STTCRMWeb.Speak" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voice-enabled CRM</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function showJson() {
            document.getElementById("txtJsonResponse").style = "display: ''; width:800px; height:300px";            
        }
    </script>
</head>
<body style="background: url(sttback.jpg) no-repeat center center fixed; background-size: 100% 100%;">
    <form id="form1" runat="server">
        <div style="text-align: center">
            <h1>Voice-enabled CRM</h1>
            <br />
            <asp:ImageButton ID="btnSpeak" runat="server" OnClick="btnSpeak_Click" Text="Speak" Height="100px" ImageUrl="~/record.png" />
            <br />
            <br />
            <br />
            <i><span lang="EN-SG" style="font-size: 11.0pt; font-family: &quot; calibri&quot; ,sans-serif; mso-fareast-font-family: Calibri; mso-fareast-theme-font: minor-latin; mso-ansi-language: EN-SG; mso-fareast-language: EN-AU; mso-bidi-language: AR-SA">I met <u>&lt;Customer Name&gt; </u>&nbsp;of <u>&lt;Company Name&gt;</u>. He/She is looking for a <u>&lt;solution&gt;</u> &nbsp;for her <u>&lt;Users/Department&gt;</u>. Estimate deal size of <u>&lt;Dollar Value&gt;</u>, RFP to be released in <u>&lt;Month and Year&gt;</u>, contract to be awarded in <u>&lt; Month and Year&gt;</u>. Please set up <u>&lt;Next Step: Meeting, Demo, Opportunity Qualification etc.&gt;</u> next week. </span></i>
            <br />
            <br />
            <table width="100%">
                <tr>
                    <td width="47%">
                        <asp:TextBox ID="txtSpeechText" runat="server" Height="306px" TextMode="MultiLine" Width="100%"></asp:TextBox>


                    </td>
                    <td width="6%"></td>
                    <td width="47%">
                        <asp:TextBox ID="txtResponse" runat="server" Height="309px" TextMode="MultiLine" Width="100%"></asp:TextBox></td>

                </tr>
                <tr>
                    <td><input type="button" value="Show Response" style="background-color:cornflowerblue; color:white" onclick="javascript:showJson()" /></td>
                    <td>
                        <asp:ImageButton ID="btnFix" ImageUrl="~/fix.png" Height="100px" runat="server" OnClick="btnFix_Click" /></td>
                    <td>
                        <asp:Label ID="txtCRMPost" runat="server"></asp:Label></td>


                </tr>
                <tr>
                    <td colspan="3" style="width:100%">
                        <asp:TextBox ID="txtJsonResponse" style="display:none" Width="800px" runat="server" Height="288px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <br />

        </div>
    </form>
</body>
</html>
