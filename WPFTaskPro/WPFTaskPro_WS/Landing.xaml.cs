using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
        }

        string currentDate = DateTime.Today.ToString("dd/mm/yyyy");
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

        }
        private void btn_list_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).CommandParameter;
            listselected = new ConvertRow().list(sw.listReadById(id));
            Refresh();
            tb_newtask.IsReadOnly = false;
            btnewtask.IsEnabled = true;
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
        }
        private void btn_newtask(object sender, RoutedEventArgs e)
        {
            string newtarea = tb_newtask.Text;
            string mess = sw.taskCreate(newtarea, "","NOT STARTED", "N","N","","","MEDIUM", listselected.id);
            MessageBox.Show(mess);
            tb_newtask.Text = "";
            Refresh();
            btnewtask.IsEnabled = false;
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
        public void RefreshTagList()
        {
            List<tag> taglist = new ConvertRow().listtotag(sw.tagReadByListId(this.taskselected.list_id));


            DGTagsList.ItemsSource = taglist;
        }
        private void btn_newMember(object sender, RoutedEventArgs e)
        {
        }
        private void btn_deletemember_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btn_newtimetrack(object sender, RoutedEventArgs e)
        {
            string startDate = tb_newTimeTrackStartTime.Text;
            string endDate = tb_newTimeTrackEndTime.Text;

            string mess = sw.timeTrackCreate(startDate, endDate,"Y",this.user.id,taskselected.id);
            MessageBox.Show(mess);
            //refresh()
        }
        private void btn_newAttachment(object sender, RoutedEventArgs e)
        {
            string newAttachment = tb_newAttachmentFilename.Text;
            string newAFilenameLink = tb_newAttachmentLink.Text;
            string mess = sw.attachmentCreate(currentDate, newAttachment, newAFilenameLink, this.user.id, taskselected.id);
            MessageBox.Show(mess);
            tb_newAttachmentFilename.Text = "";
            tb_newAttachmentLink.Text = "";
            //Refresh();
        }
        private void btn_newComment(object sender, RoutedEventArgs e)
        {
            string comment = tb_newComment.Text;
            string mess = sw.commentCreate(currentDate, comment, this.user.id, taskselected.id);
            MessageBox.Show(mess);
            tb_newComment.Text = "";
            //refresh()
        }
        private void btn_newTagList(object sender, RoutedEventArgs e)
        {
            string newTag = tb_newTagList.Text;
            string mess = sw.tagCreate(newTag, listselected.id);
            MessageBox.Show(mess);
            tb_newTagList.Text = "";
            //refresh();
        }
        private void btn_newTagTask(object sender, RoutedEventArgs e)
        {
            int i = tb_newCBTagTask.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("Seleccione un tag");
            }
            else
            {
                List<tasktag> tasktags = new ConvertRow().listtotasktag(sw.taskTagReadByTaskId(taskselected.id));
                string mess = sw.taskTagCreate(tasktags[i].tag_id, tasktags[i].task_id);
                MessageBox.Show(mess);
                //refresh()
            }
        }
    }
}
