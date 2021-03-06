﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace W2ItemHelp
{
    public partial class MainForm : Form
    {
        private STRUCT_ITEMHELP[]   g_pItemHelp = default(STRUCT_ITEMHELP[]);

        private int                 irg_ItemCount = 0;

        public MainForm()
        {
            InitializeComponent();

            Begin();
        }

        private void Begin()
        {
            g_pItemHelp = new STRUCT_ITEMHELP[BASE.MAX_ITEMLIST];

            for (int i = 0; i < g_pItemHelp.Length; i++)
            {
                g_pItemHelp[i] = STRUCT_ITEMHELP.CraftProperties();
            }

            BASE_OpenItemHelp();
        }

        private void BASE_OpenItemHelp()
        {
            const string _Path_ItemHelp_File_ = "ItemHelp.dat";

            try
            {
                
                using (var Sr = new StreamReader(_Path_ItemHelp_File_, Encoding.Default))
                {
                    int ObjectIndex = 0;
                    int LineCount = 0;

                    string Output = null;

                    while ((Output = Sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(Output) || Output.Substring(0, 1).Equals("#")) continue;
                        else
                        {
                            var ReadConfig = Output.Split(new char[] { ' ' });

                            if (ReadConfig == null) continue;

                            if (ReadConfig.Length == 1)
                            {
                                bool _isNumber = int.TryParse(ReadConfig[0], out ObjectIndex);

                                if (ObjectIndex >= BASE.MAX_ITEMLIST) continue;

                                if (_isNumber)
                                {
                                    ItemBox.Items.Add(ObjectIndex);

                                    LineCount = 0;
                                }
                                else
                                {
                                    ObjectIndex = 0;
                                }

                                continue;
                            }

                            if (ObjectIndex <= 0) continue;

                            g_pItemHelp[ObjectIndex].Index = ObjectIndex;

                            if (ReadConfig.Length == 2)
                            {
                                /* altera os "underline" da linha para espaço */

                                ReadConfig[1] = ReadConfig[1].Replace('_', ' ');

                                g_pItemHelp[ObjectIndex].AddLine(LineCount, ReadConfig[1], ReadConfig[0]);

                                LineCount++;
                            }
                        }
                    }
                }

                /* organiza a lista de itens disponíveis */

                var _arrayList = new ArrayList(ItemBox.Items);

                _arrayList.Sort();

                ItemBox.Items.Clear();

                for (int i = 0; i < _arrayList.Count; i++)
                {
                    ItemBox.Items.Add(_arrayList[i]); 
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "BASE_OpenItemHelp()");
            }
        }

        private void ItemBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Index = 0;

            int.TryParse(ItemBox.Items[ItemBox.SelectedIndex].ToString(), out Index);

            if (Index <= 0 || Index >= BASE.MAX_ITEMLIST)
            {
                throw new ArgumentOutOfRangeException("ItemBox Out Of Range");
            }

            ClearFields();

            var Content = g_pItemHelp[Index].Line;

            C0.Text = Content[00].Color;
            M0.Text = Content[00].Message;

            C1.Text = Content[01].Color;
            M1.Text = Content[01].Message;

            C2.Text = Content[02].Color;
            M2.Text = Content[02].Message;

            C3.Text = Content[03].Color;
            M3.Text = Content[03].Message;

            C4.Text = Content[04].Color;
            M4.Text = Content[04].Message;

            C5.Text = Content[05].Color;
            M5.Text = Content[05].Message;

            C6.Text = Content[06].Color;
            M6.Text = Content[06].Message;

            C7.Text = Content[07].Color;
            M7.Text = Content[07].Message;

            C8.Text = Content[08].Color;
            M8.Text = Content[08].Message;

            C9.Text = Content[09].Color;
            M9.Text = Content[09].Message;
        }


        public static IEnumerable<T> GetFormControl<T>(Control _control) where T : Control
        {
            var _CurControl = _control as T;

            if (_CurControl != null) yield return _CurControl;

            var Content = _control as ContainerControl;

            if (Content != null)
            {
                foreach (Control c in Content.Controls)
                {
                    foreach (var i in GetFormControl<T>(c))
                    {
                        yield return i;

                    }
                }
            }
        }

        private void ClearFields()
        {
            foreach (var Box in GetFormControl<TextBox>(this))
            {
                Box.Text = string.Empty;
            }
        }
    }
}
