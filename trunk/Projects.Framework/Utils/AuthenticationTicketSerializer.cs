using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Security;
using Projects.Tool;

namespace Projects.Framework.Utils
{
    internal static class AuthenticationTicketSerializer
    {
        public static FormsAuthenticationTicket Deserialize(byte[] serializedTicket, int serializedTicketLength)
        {
            FormsAuthenticationTicket ticket;
            try
            {
                using (MemoryStream stream = new MemoryStream(serializedTicket))
                {
                    using (SerializingBinaryReader reader = new SerializingBinaryReader(stream))
                    {
                        int version;
                        DateTime issueDate;
                        DateTime expirationDate;

                        if (reader.ReadByte() != 1)
                        {
                            return null;
                        }
                        version = reader.ReadByte();
                        int issueDateTimestamp = reader.ReadInt32();
                        issueDate = DateTimeExtend.FromUnixTime(issueDateTimestamp);

                        int expirationTimestamp = reader.ReadInt32();
                        expirationDate = DateTimeExtend.FromUnixTime(expirationTimestamp);
                        bool isPersistent = reader.ReadByte() == 1;

                        if (reader.ReadByte() != 0xfe)
                        {
                            return null;
                        }

                        string name = reader.ReadBinaryString();
                        string userData = reader.ReadBinaryString();
                        string cookiePath = reader.ReadBinaryString();
                        if (reader.ReadByte() != 0xff)
                        {
                            return null;
                        }
                        if (stream.Position != serializedTicketLength)
                        {
                            return null;
                        }
                        ticket = new FormsAuthenticationTicket(version, name, issueDate, expirationDate, isPersistent, userData, cookiePath);
                    }
                }
            }
            catch
            {
                ticket = null;
            }
            return ticket;
        }

        public static byte[] Serialize(FormsAuthenticationTicket ticket)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                using (SerializingBinaryWriter writer = new SerializingBinaryWriter(stream))
                {
                    writer.Write((byte)1);
                    writer.Write((byte)ticket.Version);
                    writer.Write((int)ticket.IssueDate.ToUnixTime());
                    writer.Write((int)ticket.Expiration.ToUnixTime());
                    writer.Write(ticket.IsPersistent);
                    writer.Write((byte)0xfe);
                    writer.WriteBinaryString(ticket.Name);
                    writer.WriteBinaryString(ticket.UserData);
                    writer.WriteBinaryString(ticket.CookiePath);
                    writer.Write((byte)0xff);
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }

        private sealed class SerializingBinaryReader : BinaryReader
        {
            public SerializingBinaryReader(Stream input)
                : base(input)
            {
            }

            public string ReadBinaryString()
            {
                int num = base.Read7BitEncodedInt();
                byte[] buffer = this.ReadBytes(num * 2);
                char[] chArray = new char[num];
                for (int i = 0; i < chArray.Length; i++)
                {
                    chArray[i] = (char)(buffer[2 * i] | (buffer[(2 * i) + 1] << 8));
                }
                return new string(chArray);
            }

            public override string ReadString()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class SerializingBinaryWriter : BinaryWriter
        {
            public SerializingBinaryWriter(Stream output)
                : base(output)
            {
            }

            public override void Write(string value)
            {
                throw new NotImplementedException();
            }

            public void WriteBinaryString(string value)
            {
                byte[] buffer = new byte[value.Length * 2];
                for (int i = 0; i < value.Length; i++)
                {
                    char ch = value[i];
                    buffer[2 * i] = (byte)ch;
                    buffer[(2 * i) + 1] = (byte)(ch >> 8);
                }
                base.Write7BitEncodedInt(value.Length);
                this.Write(buffer);
            }
        }
    }
}
