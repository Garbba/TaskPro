using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFTaskPro_WS.O;

namespace WPFTaskPro_WS
{
    /// <summary>
    /// Lógica de interacción para Landing.xaml
    /// </summary>
    public partial class Landing : Window
    {
        public Landing(user u)
        {
            InitializeComponent();
            tbuser.Text = u.username;
            listselected = null;
            this.user = u;
            DGListPreCharge.ItemsSource = new ConvertRow().listprecharged();
            tb_newtask.IsReadOnly = true;
            btnewtask.IsEnabled = false;
            Refresh();
            contentGrid.Visibility = Visibility.Collapsed;
        }

        string currentDate = DateTime.Today.ToString("dd/MM/yyyy");
        user user;
        list listselected;
        task taskselected;
        SWRef.WebService1SoapClient sw = new SWRef.WebService1SoapClient();

        private void Refresh()
        {
            try
            {
                DGListas.ItemsSource = new ConvertRow().listtolist(sw.listReadByUserID(this.user.id));
            }
            catch{}
            if (listselected == null) {}
            else
            {
                DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByListId(this.listselected.id));
                titulo_tareas.Text =  $"Tareas de la lista {this.listselected.listName}";
            }
            
        }
        private void RefreshTask()
        {
            txtTaskTitle.Text = taskselected.title;
            txtTaskDescription.Text = taskselected.taskdescription;
            cbTaskFavorite.SelectedIndex = Convert.ToInt32(taskselected.isfavorite);
            cbTaskOnMyDay.SelectedIndex = Convert.ToInt32(taskselected.isonmyday);

            DateTime? sd = taskselected.startdate;
            string ssd = "";
            if (sd != null) ssd = sd.Value.ToString("dd/MM/yyyy");
            txtTaskStartDate.Text = ssd;

            DateTime? ed = taskselected.enddate;
            string sed = "";
            if (ed != null) sed = ed.Value.ToString("dd/MM/yyyy");
            txtTaskEndDate.Text = sed;

            int idPri = -1;
            switch (taskselected.taskPriority)
            {
                case "LOW": idPri = 0; break;
                case "MEDIUM": idPri = 1; break;
                case "IMPORTANT": idPri = 2; break;
                case "URGENT": idPri = 3; break;
            }
            cbTaskPriority.SelectedIndex = idPri;
            
            int idStat = -1;
            switch (taskselected.taskStatus)
            {
                case "NOT STARTED": idStat = 0; break;
                case "IN PROGRESS": idStat = 1; break;
                case "COMPLETED": idStat = 2; break;
            }
            cbTaskStatus.SelectedIndex = idStat;
            RefreshTagList();
        }
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            new EditUser(user).Show();
            this.Close();
        }
        private void btn_listprecharged_Click(object sender, RoutedEventArgs e)
        {
            listselected = null;
            tb_newtask.IsReadOnly = true;
            var id = (int)((Button)sender).CommandParameter;
            switch (id)
            {
                case 0:
                    //On My day
                    DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByUserOnMyDay(this.user.id));
                    titulo_tareas.Text = $"Tareas de tu dia";
                    break;
                case 1:
                    //Favorite
                    DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByUserFavorite(this.user.id));
                    titulo_tareas.Text = $"Tareas favoritas";
                    break;
                case 2:
                    //Assigned to me
                    DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByUser(this.user.id));
                    titulo_tareas.Text = $"Tareas asignadas a ti";
                    break;

            }
            tb_newtask.IsReadOnly = true;
            btnewtask.IsEnabled = false;
            contentGrid.Visibility = Visibility.Collapsed;
        }
        private void btn_list_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).CommandParameter;
            listselected = new ConvertRow().list(sw.listReadById(id));
            Refresh();
            tb_newtask.IsReadOnly = false;
            btnewtask.IsEnabled = true;
            contentGrid.Visibility = Visibility.Collapsed;
        }
        private void btn_editlist_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).CommandParameter;
            new ListEdit(new ConvertRow().list(sw.listReadById(id)), this.user).Show();
            this.Close();
        }
        private void btn_newlist(object sender, RoutedEventArgs e)
        {
            string newlist = tb_newlist.Text;
            string commentlist = sw.listCreate(newlist);
            MessageBox.Show(commentlist);
            if (commentlist == "Lista agregada correctamente")
            {
                int idlastlist = sw.listReadAll().Tables[0].Rows.Count - 1;
                DataRow lastlist = sw.listReadAll().Tables[0].Rows[idlastlist];
                int listid = int.Parse(lastlist["id"].ToString());

                sw.listAccessCreateUpdate(this.user.id, listid, "OWNER");
            }
            tb_newlist.Text = "";
            Refresh();
        }

        private void btn_task_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).CommandParameter;
            contentGrid.Visibility = Visibility.Visible;
            this.taskselected = new ConvertRow().task(sw.taskReadById(id));
            RefreshTask();
            RefreshTagTask();
            RefreshTagList();
            RefreshMembers();
            RefreshComments();
            RefreshAttachment();
            RefreshTimeTrack();
            taglistselected = null;
            List<tag> taglist = new ConvertRow().listtotag(sw.tagReadByListId(this.taskselected.list_id));
            tb_newCBTagTask.ItemsSource = taglist;
        }

        private void btn_newtask(object sender, RoutedEventArgs e)
        {
            string newtarea = tb_newtask.Text;
            string mess = sw.taskCreate(newtarea, "","NOT STARTED", "N","N","","","MEDIUM", listselected.id);
            MessageBox.Show(mess);
            tb_newtask.Text = "";
            Refresh();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Visibility = Visibility.Collapsed;
        }
        private void TaskUpdate_Click(object sender, RoutedEventArgs e)
        {

            string title = txtTaskTitle.Text;
            string descrip = txtTaskDescription.Text;

            string sd = txtTaskStartDate.Text;
            string ed = txtTaskEndDate.Text;
            string[] booleana = new string[] { "N", "Y" };
            string isFav = booleana[cbTaskFavorite.SelectedIndex];
            string isOMD = booleana[cbTaskOnMyDay.SelectedIndex];

            string[] stat = new string[] { "NOT STARTED", "IN PROGRESS","COMPLETED" };
            string status = stat[cbTaskStatus.SelectedIndex];

            string[] prio = new string[] { "LOW", "MEDIUM", "IMPORTANT", "URGENT" };
            string priority = prio[cbTaskPriority.SelectedIndex];

            string mess = sw.taskUpdate(this.taskselected.id,title,descrip,status,isFav,isOMD,sd,ed,priority,this.taskselected.list_id);
            MessageBox.Show(mess);
            Refresh();
            this.taskselected = new ConvertRow().task(sw.taskReadById(this.taskselected.id));
        }

        tag taglistselected = null;
        public void RefreshTagList()
        {
            List<tag> taglist = new ConvertRow().listtotag(sw.tagReadByListId(this.taskselected.list_id));
            DGTagsList.ItemsSource = taglist;
        }
        private void btn_newTagList(object sender, RoutedEventArgs e)
        {
            string newTag = tb_newTagList.Text;
            if (taglistselected == null) {
                string mess = sw.tagCreate(newTag, listselected.id);
                MessageBox.Show(mess);
                tb_newTagList.Text = "";
                
            } else
            {
                string mess = sw.tagUpdate(taglistselected.id, newTag, listselected.id);
                MessageBox.Show(mess);
                tb_newTagList.Text = "";
                taglistselected = null;
                btnAddTagList.Text = "Add";
            }
            RefreshTagList();
            RefreshTagTask();
        }
        private void btn_taglist_Click(object sender, RoutedEventArgs e)
        {
            var tagselected = (tag)((Button)sender).CommandParameter;
            btnAddTagList.Text= "Update";
            taglistselected = tagselected;
            tb_newTagList.Text = tagselected.tagName;
            RefreshTagTask();
        }
        private void btn_deletetaglist_Click(object sender, RoutedEventArgs e)
        {
            var tagselected = (tag)((Button)sender).CommandParameter;
            string mess = sw.tagDelete(tagselected.id);
            MessageBox.Show(mess);
            RefreshTagList();
            tb_newTagList.Text = "";
            taglistselected = null;
            btnAddTagList.Text = "Add";
            RefreshTagTask();
        }

        public void RefreshTagTask()
        {
            DataSet ds = sw.taskTagReadByTaskId(this.taskselected.id);
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<tasktag> tagtasks = new ConvertRow().listtotasktag(ds);
                List<tasktag_name> tagtasksname = new List<tasktag_name>();
                foreach(tasktag tt in tagtasks)
                {
                    tasktag_name t = new tasktag_name();
                    t.tasktag = tt;
                    t.name = new ConvertRow().tag(sw.tagReadById(tt.tag_id)).tagName.ToString();
                    
                    tagtasksname.Add(t);
                }
                DGTagTask.ItemsSource = tagtasksname;

            }
            else
            {
                DGTagTask.ItemsSource = null;

            }
            List<tag> taglist = new ConvertRow().listtotag(sw.tagReadByListId(this.taskselected.list_id));
            tb_newCBTagTask.ItemsSource = taglist;
        }
        private void btn_newTagTask(object sender, RoutedEventArgs e)
        {
            int i = tb_newCBTagTask.SelectedIndex;
            List<tag> taglist = new List<tag>();


            if (i == -1)
                {
                    MessageBox.Show("Seleccione un tag");
                }
                else
                {
                taglist = new ConvertRow().listtotag(sw.tagReadByListId(this.taskselected.list_id));
                string mess = sw.taskTagCreate(taglist[i].id, this.taskselected.id);
                    MessageBox.Show(mess);
                    //refresh()
                }
            
            RefreshTagTask();

        }
        private void btn_deletetagtask_Click(object sender, RoutedEventArgs e)
        {
            var tagselected = (tasktag)((Button)sender).CommandParameter;
            string mess = sw.taskTagDelete(tagselected.tag_id, tagselected.task_id);
            
            RefreshTagTask();
            tb_newCBTagTask.SelectedIndex = -1;
            MessageBox.Show(mess);
        }

        public void RefreshMembers()
        {
            DataSet ds = sw.memberReadByTaskId(this.taskselected.id);
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<member> memb = new ConvertRow().listtomember(ds);
                List<member_name> membname = new List<member_name>();
                foreach(member mm in memb)
                {
                    member_name m = new member_name();
                    m.member = mm;
                    m.name = new ConvertRow().user(sw.userReadById(mm.user_id)).nickname;

                    membname.Add(m);
                }
                DGMiembros.ItemsSource = membname;

            }
            else
            {
                DGMiembros.ItemsSource = null;
            }
        }
        private void btn_newMember(object sender, RoutedEventArgs e)
        {
            DataSet ds = sw.userReadByNickname(tb_newMember.Text);
            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("El usuario no existe");
                tb_newMember.Text = "";
            }
            else
            {
                int u = new ConvertRow().user(ds).id;
                string mess = sw.memberCreate(u, this.taskselected.id);
                MessageBox.Show(mess);
                tb_newMember.Text = "";
                RefreshMembers();
            }
        }
        private void btn_deletemember_Click(object sender, RoutedEventArgs e)
        {
            var m = (member)((Button)sender).CommandParameter;
            string mess = sw.memberDelete(m.user_id, m.task_id);
            RefreshMembers();
            tb_newMember.Text = "";
            MessageBox.Show(mess);
        }

        public void RefreshComments()
        {
            List<comment> taglist = new ConvertRow().listtocomment(sw.commentReadByTaskId(this.taskselected.id));
            DGComment.ItemsSource = taglist;
        }
        private void btn_newComment(object sender, RoutedEventArgs e)
        {
            string comment = tb_newComment.Text;
            string date = this.currentDate;
            string mess = sw.commentCreate(date, comment, this.user.id, taskselected.id);
            MessageBox.Show(mess);
            tb_newComment.Text = "";
            RefreshComments();
        }
        

        attachment attachmentselected = null;
        public void RefreshAttachment()
        {
            List<attachment> a = new ConvertRow().listtoattachment(sw.attachmentReadByTaskId(this.taskselected.id));
            DGAttachment.ItemsSource = a;
        }
        private void btn_newAttachment(object sender, RoutedEventArgs e)
        {
            string newAttachment = tb_newAttachmentFilename.Text;
            string newAFilenameLink = tb_newAttachmentLink.Text;
            if (attachmentselected == null)
            {
                string mess = sw.attachmentCreate(currentDate, newAttachment, newAFilenameLink, this.user.id, taskselected.id);
                MessageBox.Show(mess);
                tb_newAttachmentFilename.Text = "";
                tb_newAttachmentLink.Text = "";
                attachmentselected = null;
            }
            else
            {
                string mess = sw.attachmentUpdate(attachmentselected.id, currentDate, newAttachment, newAFilenameLink, this.user.id, taskselected.id);
                MessageBox.Show(mess);
                attachmentselected = null;
                tb_newAttachmentFilename.Text = "";
                tb_newAttachmentLink.Text = "";
                btnattachment.Text = "Add";
            }
            RefreshAttachment();
        }
        private void btn_attachment_Click(object sender, RoutedEventArgs e)
        {
            var a = (attachment)((Button)sender).CommandParameter;
            btnattachment.Text = "Update";
            tb_newAttachmentFilename.Text = a.attachmentFilename;
            tb_newAttachmentLink.Text = a.attachmentLink;
            attachmentselected = a;
            RefreshAttachment();
        }
        private void btn_deleteattachment_Click(object sender, RoutedEventArgs e)
        {
            var a = (attachment)((Button)sender).CommandParameter;
            string mess = sw.attachmentDelete(a.id);
            MessageBox.Show(mess);
            RefreshAttachment();
            attachmentselected = null;
            tb_newAttachmentFilename.Text = "";
            tb_newAttachmentLink.Text = "";
            btnattachment.Text = "Add";
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        timetrack timetrackselected = null;
        public void RefreshTimeTrack()
        {
            DataSet ds = sw.timeTrackReadByTaskId(this.taskselected.id);
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<timetrack> timetracks = new ConvertRow().listtotimetrack(ds);
                List<timetrack_duration> timetrackduration = new List<timetrack_duration>();
                foreach (timetrack tt in timetracks)
                {
                    timetrack_duration t = new timetrack_duration();
                    t.timetrack = tt;
                    t.duration = Duration(tt.starttime,tt.endtime);

                    timetrackduration.Add(t);
                }
                DGTimeTrack.ItemsSource = timetrackduration;
            }
            else
            {
                DGTimeTrack.ItemsSource = null;

            }
        }
        private void btn_newtimetrack(object sender, RoutedEventArgs e)
        {
            string startDate = tb_newTimeTrackStartTime.Text;
            string endDate = tb_newTimeTrackEndTime.Text;
            tb_newTimeTrackStartTime.Text = "";
            tb_newTimeTrackEndTime.Text = "";
            btnattachment.Text = "Add";
            string mess = "";
            if (attachmentselected == null)
            {
                mess = sw.timeTrackCreate(startDate, endDate, "Y", this.user.id, taskselected.id);
            }
            else
            {
                mess = sw.timeTrackUpdate(timetrackselected.id, startDate, endDate, "Y", this.user.id, taskselected.id);
            }
            MessageBox.Show(mess);
            timetrackselected = null;
            RefreshTimeTrack();
        }
        private string Duration(DateTime startTime, DateTime endTime)
        {
            TimeSpan timeSpan = endTime - startTime;

            int days = timeSpan.Days;
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;

            return $"{days} días, {hours} horas y {minutes} minutos";
        }

        private void btn_timetrack_Click(object sender, RoutedEventArgs e)
            {
            var tt = (timetrack)((Button)sender).CommandParameter;
            tb_newTimeTrackStartTime.Text = tt.starttime.ToString("dd/MM/yyyy HH:mm:ss");
            tb_newTimeTrackEndTime.Text = tt.endtime.ToString("dd/MM/yyyy HH:mm:ss");
            btnattachment.Text = "Update";
            timetrackselected = tt;
            RefreshTimeTrack();
        }
        private void btn_deletetimetrack_Click(object sender, RoutedEventArgs e)
        {
            var tt = (timetrack)((Button)sender).CommandParameter;
            string mess = sw.timeTrackDelete(tt.id);
            MessageBox.Show(mess);
            RefreshTimeTrack();
            tb_newTimeTrackStartTime.Text = "";
            tb_newTimeTrackEndTime.Text = "";
            btnattachment.Text = "Add";
            timetrackselected = null;
        }
    }
}
