/*
	Copyright 2011 MCForge
		
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections;

namespace MCForge.Gui
{
    public partial class LavaMapBrowser : Form
    {
        private bool listing = false, loadingDet = false;
        private string downloadUrl = "http://www.mcforge.net/lavamaps/dl.php";
        private string listUrl = "http://www.mcforge.net/lavamaps/listdata.php";
        private string imgUrl = "http://www.mcforge.net/lavamaps/thumb.php";
        private System.Timers.Timer downloadTextTimer;
        private System.Threading.Thread downloadThread, listingThread, detailsThread;
        private Serializer serializer = new Serializer();
        private LavaMapCollection lmc = new LavaMapCollection(new LavaMapListView());

        public LavaMapBrowser()
        {
            InitializeComponent();
        }

        private void LavaMapBrowser_Load(object sender, EventArgs e)
        {
            downloadTextTimer = new System.Timers.Timer(500);
            downloadTextTimer.Elapsed += delegate
            {
                downloadTextUpdate();
            };
            updateMapList(String.Empty, true);
        }

        private void LavaMapBrowser_Unload(object sender, EventArgs e)
        {
            // TRY-CATCH FIXES ALL DERPNESS
            try
            {
                downloadTextTimer.Dispose();
            }
            catch { }
        }

        private void downloadTextUpdate()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { downloadTextUpdate(); }));
                    return;
                }

                if (btnDownload.Text.EndsWith("..."))
                    btnDownload.Text = "Downloading   ";
                else if (btnDownload.Text.EndsWith(".. "))
                    btnDownload.Text = "Downloading...";
                else if (btnDownload.Text.EndsWith(".  "))
                    btnDownload.Text = "Downloading.. ";
                else
                    btnDownload.Text = "Downloading.  ";
            }
            catch { }
        }

        private void downloadBtnReset()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { downloadBtnReset(); }));
                    return;
                }

                btnDownload.Text = "Download";
                btnDownload.Enabled = true;
            }
            catch { }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                updateMapList(txtSearch.Text, true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            updateMapList(txtSearch.Text, true);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            downloadMap(GetSelectedLavaMap());
        }

        private void dgvMaps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LavaMapBrowserData map = GetSelectedLavaMap();
            if (map != null)
                loadMapDetails(map, true);
        }

        private void dgvMaps_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            downloadMap(GetSelectedLavaMap());
        }

        private void lnkSubmitMap_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.mcforge.net/lavamaps/submit");
            }
            catch
            {
                MessageBox.Show("Failed to open link!");
            }
        }

        private void updateMapList(string search, bool thread = false) {
            if (this.listing) return;
            if (thread)
            {
                listingThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    updateMapList(search);
                }));
                listingThread.Start();
                return;
            }

            try
            {
                this.listing = true;
                string data = String.Empty;
                using (WebClient WEB = new WebClient())
                    data = WEB.DownloadString(listUrl + "?search=" + Heart.UrlEncode(search));

                if (String.IsNullOrEmpty(data))
                {
                    MessageBox.Show("No data was recieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto end;
                }
                if (data.ToLower().StartsWith("error"))
                {
                    MessageBox.Show(data, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto end;
                }

                ArrayList dataObj = (ArrayList)serializer.Deserialize(data);
                lmc.Clear();
                foreach (object obj in dataObj)
                {
                    Hashtable tbl = (Hashtable)obj;
                    lmc.Add(new LavaMapBrowserData(Convert.ToInt32(tbl["id"]), Convert.ToInt32(tbl["time"]), tbl["name"].ToString(), tbl["author"].ToString(), tbl["desc"].ToString(), tbl["image_location"].ToString()));
                }

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        dgvMaps.DataSource = null;
                        dgvMaps.DataSource = lmc;
                        foreach (DataGridViewColumn column in dgvMaps.Columns)
                            column.Width = 108;
                    }));
                    goto end;
                }

                dgvMaps.DataSource = null;
                dgvMaps.DataSource = lmc;
                foreach (DataGridViewColumn column in dgvMaps.Columns)
                    column.Width = 108;

                end:
                this.listing = false;
            }
            catch (Exception ex) { this.listing = false; Server.ErrorLog(ex); MessageBox.Show("An unknown error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void loadMapDetails(LavaMapBrowserData map, bool thread = false)
        {
            if (this.loadingDet) return;
            if (thread)
            {
                detailsThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    loadMapDetails(map);
                }));
                detailsThread.Start();
                return;
            }

            try
            {
                this.loadingDet = true;
                Image img = null;

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        txtDesc.Text = map.desc;
                    }));
                }
                else
                {
                    txtDesc.Text = map.desc;
                }

                using (WebClient WEB = new WebClient())
                    using (Stream stream = WEB.OpenRead(imgUrl + "?file=" + Heart.UrlEncode(map.image) + "&width=150&height=150&force&png"))
                        img = Image.FromStream(stream);

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        pbImage.Width = img.Width;
                        pbImage.Height = img.Height;
                        pbImage.Image = img;
                    }));
                }
                else
                {
                    pbImage.Width = img.Width;
                    pbImage.Height = img.Height;
                    pbImage.Image = img;
                }
                this.loadingDet = false;
            }
            catch (Exception ex) { this.loadingDet = false; Server.ErrorLog(ex); MessageBox.Show("An unknown error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void downloadMap(LavaMapBrowserData mapData)
        {
            if (mapData == null) return;
            int id = mapData.id;
            string name = mapData.name.ToLower();
            downloadThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                byte[] data;
                string dataStr;
                using (WebClient WEB = new WebClient())
                {
                    try
                    {
                        if (name == Server.mainLevel.name.ToLower())
                        {
                            MessageBox.Show("Error: You cannot overwrite the main level!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto end;
                        }
                        if (File.Exists("levels/" + name + ".lvl"))
                        {
                            if (MessageBox.Show("Map \"" + name + "\" already exists. Do you want to overwrite it?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                                goto end;
                        }

                        data = WEB.DownloadData(downloadUrl + "?id=" + id + "&mode=lvl");
                        if (data.Length < 1)
                        {
                            MessageBox.Show("No data was recieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto end;
                        }
                        dataStr = new ASCIIEncoding().GetString(data).Trim();
                        if (dataStr.ToLower().StartsWith("error") || dataStr == "")
                        {
                            MessageBox.Show(dataStr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto end;
                        }
                        FileStream fs = File.Create("levels/" + name + ".lvl");
                        fs.Write(data, 0, data.Length);
                        fs.Dispose();

                        data = WEB.DownloadData(downloadUrl + "?id=" + id + "&mode=properties");
                        if (data.Length < 1)
                        {
                            MessageBox.Show("No data was recieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto end;
                        }
                        dataStr = new ASCIIEncoding().GetString(data).Trim();
                        if (dataStr.ToLower().StartsWith("error"))
                        {
                            MessageBox.Show(dataStr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto end;
                        }
                        fs = File.Create("properties/lavasurvival/" + name + ".properties");
                        fs.Write(data, 0, data.Length);
                        fs.Dispose();

                        MessageBox.Show("Map \"" + name + "\" has been downloaded!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    end:
                        downloadTextTimer.Stop();
                        downloadBtnReset();
                    }
                    catch (Exception ex) { Server.ErrorLog(ex); MessageBox.Show("An unknown error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }));
            downloadThread.Start();

            btnDownload.Enabled = false;
            btnDownload.Text = "Downloading   ";
            downloadTextTimer.Start();
        }

        private LavaMapBrowserData GetSelectedLavaMap()
        {
            if (this.dgvMaps.SelectedRows == null)
                return null;
            if (this.dgvMaps.SelectedRows.Count < 1)
                return null;
            return (LavaMapBrowserData)this.dgvMaps.SelectedRows[0].DataBoundItem;
        }


        private class LavaMapBrowserData
        {
            public int id, time;
            public string name, author, desc, image;
            public DateTime dateTime;

            public LavaMapBrowserData(int id, int time, string name, string author, string desc, string image)
            {
                this.id = id;
                this.time = time;
                this.name = name;
                this.author = author;
                this.desc = desc;
                this.image = image;
                this.dateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(this.time);
            }
        }

        #region DataGridView Stuff
        private class LavaMapCollection : List<LavaMapBrowserData>, ITypedList
        {
            protected ILavaMapViewBuilder _viewBuilder;

            public LavaMapCollection(ILavaMapViewBuilder viewBuilder)
            {
                _viewBuilder = viewBuilder;
            }

            #region ITypedList Members

            protected PropertyDescriptorCollection _props;

            public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
            {
                if (_props == null)
                {
                    _props = _viewBuilder.GetView();
                }
                return _props;
            }

            public string GetListName(PropertyDescriptor[] listAccessors)
            {
                return ""; // was used by 1.1 datagrid
            }

            #endregion
        }

        private interface ILavaMapViewBuilder
        {
            PropertyDescriptorCollection GetView();
        }

        private class LavaMapListView : ILavaMapViewBuilder
        {
            public PropertyDescriptorCollection GetView()
            {
                List<PropertyDescriptor> props = new List<PropertyDescriptor>();
                LavaMapMethodDelegate del = delegate(LavaMapBrowserData data)
                {
                    return data.name;
                };
                props.Add(new LavaMapMethodDescriptor("Map Name", del, typeof(string)));

                del = delegate(LavaMapBrowserData data)
                {
                    return data.author;
                };
                props.Add(new LavaMapMethodDescriptor("Submitted By", del, typeof(string)));

                del = delegate(LavaMapBrowserData data)
                {
                    return data.dateTime.ToString("MM/dd/yyyy hh:mm tt");
                };
                props.Add(new LavaMapMethodDescriptor("Submitted On", del, typeof(string)));

                PropertyDescriptor[] propArray = new PropertyDescriptor[props.Count];
                props.CopyTo(propArray);
                return new PropertyDescriptorCollection(propArray);
            }
        }

        private delegate object LavaMapMethodDelegate(LavaMapBrowserData data);

        private class LavaMapMethodDescriptor : PropertyDescriptor
        {
            protected LavaMapMethodDelegate _method;
            protected Type _methodReturnType;

            public LavaMapMethodDescriptor(string name, LavaMapMethodDelegate method,
             Type methodReturnType)
                : base(name, null)
            {
                _method = method;
                _methodReturnType = methodReturnType;
            }

            public override object GetValue(object component)
            {
                LavaMapBrowserData l = (LavaMapBrowserData)component;
                return _method(l);
            }

            public override Type ComponentType
            {
                get { return typeof(LavaMapBrowserData); }
            }

            public override Type PropertyType
            {
                get { return _methodReturnType; }
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override void ResetValue(object component) { }

            public override bool IsReadOnly
            {
                get { return true; }
            }

            public override void SetValue(object component, object value) { }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }
        #endregion
    }
}
