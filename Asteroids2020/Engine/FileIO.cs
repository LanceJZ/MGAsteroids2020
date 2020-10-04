using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Panther
{
    class FileIO
    {
        #region Fields
        FileStream fileStream;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public FileIO()
        {

        }
        #endregion
        #region Public Methods
        public bool DoesFileExist(string fileName)
        {
            if (File.Exists(fileName))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Returns string of data from file. Filename must include path and extension.
        /// </summary>
        /// <param name="fileName">path + filename + extension.</param>
        /// <returns>Data as string.</returns>
        public string ReadStringFile(string fileName)
        {
            string data = "";
            
            if (DoesFileExist(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] dataByte = new byte[1024];
                UTF8Encoding bufferUTF8 = new UTF8Encoding(true);

                while (fileStream.Read(dataByte, 0, dataByte.Length) > 0)
                {
                    data += bufferUTF8.GetString(dataByte, 0, dataByte.Length);
                }

                Close();
            }

            return data;
        }
        /// <summary>
        /// Converts a string to a byte array for file access.
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        public byte[] StringToByteArray(string dataString)
        {
            return new UTF8Encoding(true).GetBytes(dataString);
        }
        /// <summary>
        /// Open for Write. Must be closed after done writing.
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenForWrite(string fileName)
        {
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
        }
        /// <summary>
        /// Write a byte array, without close. Must be closed when all byte arrays are done.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        public void WriteByteArray(byte[] data)
        {
            fileStream.Write(data, 0, data.Length);
        }
        /// <summary>
        /// Sales data into file. Filename needs to include path and extension.
        /// String converted into byte array outside of function.
        /// UTF8Encoding(true).GetBytes(data);
        /// </summary>
        /// <param name="fileName">path + name of file + extension</param>
        /// <param name="data">Data to be saved as a byte array.</param>
        public void WriteStringFile(string fileName, byte[] data)
        {
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            fileStream.Write(data, 0, data.Length);
            Close();
        }
        /// <summary>
        /// Sales data into file. Filename needs to include path and extension.
        /// </summary>
        /// <param name="fileName">path + name of file + extension</param>
        /// <param name="data">Data to be saved.</param>
        public void WriteStringFile(string fileName, string data)
        {
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            byte[] databytes = new UTF8Encoding(true).GetBytes(data);
            fileStream.Write(databytes, 0, databytes.Length);
            Close();
        }
        /// <summary>
        /// Write array of vertices for Vector Model.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="fileName"></param>
        public void WriteVectorModelFile(Vector3[] vertices, string fileName)
        {
            fileName = "Content/Models/" + fileName + ".vec";
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            foreach (Vector3 vert in vertices)
            {
                byte[] verticy = new UTF8Encoding(true).GetBytes(vert.ToString());
                fileStream.Write(verticy, 0, verticy.Length);
            }

            Close();
        }
        /// <summary>
        /// Read model file and convert/decode to Vector3 Array.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Vector3[] ReadVectorModelFile(string fileName)
        {
            string dataRead = "";
            List<Vector3> vertRead = new List<Vector3>();
            fileName = "Content/Models/" + fileName + ".vec";

            if (File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] dataByte = new byte[1024];
                UTF8Encoding bufferUTF8 = new UTF8Encoding(true);

                while (fileStream.Read(dataByte, 0, dataByte.Length) > 0)
                {
                    dataRead += bufferUTF8.GetString(dataByte, 0, dataByte.Length);
                }

                Close();

                string onAxis = "";
                string number = "";
                float X = 0;
                float Y = 0;
                float Z;

                foreach (char dataChar in dataRead)
                {
                    if (dataChar.ToString() == "'\0'")
                    {
                        break;
                    }

                    if (dataChar.ToString() == "{" || dataChar.ToString() == ":")
                    {
                        continue;
                    }

                    switch (dataChar.ToString())
                    {
                        case "X":
                            onAxis = dataChar.ToString();
                            continue;
                        case "Y":
                            onAxis = dataChar.ToString();
                            continue;
                        case "Z":
                            onAxis = dataChar.ToString();
                            continue;
                    }

                    switch (onAxis)
                    {
                        case "X":
                            if (dataChar.ToString() == " ")
                            {
                                X = float.Parse(number);
                                number = "";
                                onAxis = "";
                            }
                            else
                            {
                                if (dataChar.ToString() == "}")
                                {
                                    continue;
                                }

                                number += dataChar.ToString();
                            }
                            break;
                        case "Y":
                            if (dataChar.ToString() == " ")
                            {
                                Y = float.Parse(number);
                                number = "";
                                onAxis = "";
                            }
                            else
                            {
                                if (dataChar.ToString() == "}")
                                {
                                    continue;
                                }

                                number += dataChar.ToString();
                            }
                            break;
                        case "Z":
                            if (dataChar.ToString() == "}")
                            {
                                Z = float.Parse(number);
                                vertRead.Add(new Vector3(X, Y, Z));
                                onAxis = "";
                                number = "";
                            }
                            else
                            {
                                number += dataChar.ToString();
                            }
                            break;
                    }
                }
            }
            else
            {
                //Debug file not found.
                Core.DebugConsole("File " + fileName + " not found.");
                Core.TheGame.Exit();
            }

            return vertRead.ToArray();
        }

        public void Close()
        {
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
