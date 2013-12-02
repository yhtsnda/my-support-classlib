using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace Avalon.UCenter
{
    [ServiceContractAttribute()]
    internal interface Passport91ServiceInterfaceSoap
    {
        [OperationContractAttribute(Action = "http://tempuri.org/RegisterUserInfo_Cop_591UP_WithRegType", ReplyAction = "*")]
        [XmlSerializerFormatAttribute(SupportFaults = true)]
        RegisterUserInfo_Cop_591UP_WithRegTypeResponse RegisterUserInfo_Cop_591UP_WithRegType(RegisterUserInfo_Cop_591UP_WithRegTypeRequest request);

        [OperationContractAttribute(Action = "http://tempuri.org/RegisterUserNameCheck", ReplyAction = "*")]
        [XmlSerializerFormatAttribute(SupportFaults = true)]
        RegisterUserNameCheckResponse RegisterUserNameCheck(RegisterUserNameCheckRequest request);
    }

    [MessageContractAttribute(WrapperName = "RegisterUserInfo_Cop_591UP_WithRegTypeResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    internal partial class RegisterUserInfo_Cop_591UP_WithRegTypeResponse
    {

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int RegisterUserInfo_Cop_591UP_WithRegTypeResult;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public uint return_userid;

        public RegisterUserInfo_Cop_591UP_WithRegTypeResponse()
        {
        }

        public RegisterUserInfo_Cop_591UP_WithRegTypeResponse(int RegisterUserInfo_Cop_591UP_WithRegTypeResult, uint return_userid)
        {
            this.RegisterUserInfo_Cop_591UP_WithRegTypeResult = RegisterUserInfo_Cop_591UP_WithRegTypeResult;
            this.return_userid = return_userid;
        }
    }

    [MessageContractAttribute(WrapperName = "RegisterUserInfo_Cop_591UP_WithRegType", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    internal partial class RegisterUserInfo_Cop_591UP_WithRegTypeRequest
    {
        [MessageHeaderAttribute(Namespace = "http://tempuri.org/")]
        public UserNameToken UserNameToken;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public string userName;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public string password;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 2)]
        public string nickName;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 3)]
        public string mobile;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 4)]
        public string ipAddress;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 5)]
        public int regType;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 6)]
        public string checkCode;

        public RegisterUserInfo_Cop_591UP_WithRegTypeRequest()
        {
        }

        public RegisterUserInfo_Cop_591UP_WithRegTypeRequest(UserNameToken UserNameToken, string userName, string password, string nickName, string mobile, string ipAddress, int regType, string checkCode)
        {
            this.UserNameToken = UserNameToken;
            this.userName = userName;
            this.password = password;
            this.nickName = nickName;
            this.mobile = mobile;
            this.ipAddress = ipAddress;
            this.regType = regType;
            this.checkCode = checkCode;
        }
    }

    [MessageContractAttribute(WrapperName = "RegisterUserNameCheck", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    internal partial class RegisterUserNameCheckRequest
    {

        [MessageHeaderAttribute(Namespace = "http://tempuri.org/")]
        public UserNameToken UserNameToken;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public string username;

        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 1)]
        public string checkcode;

        public RegisterUserNameCheckRequest()
        {
        }

        public RegisterUserNameCheckRequest(UserNameToken UserNameToken, string username, string checkcode)
        {
            this.UserNameToken = UserNameToken;
            this.username = username;
            this.checkcode = checkcode;
        }
    }

    [MessageContractAttribute(WrapperName = "RegisterUserNameCheckResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
    internal partial class RegisterUserNameCheckResponse
    {
        [MessageBodyMemberAttribute(Namespace = "http://tempuri.org/", Order = 0)]
        public int RegisterUserNameCheckResult;

        public RegisterUserNameCheckResponse()
        {
        }

        public RegisterUserNameCheckResponse(int RegisterUserNameCheckResult)
        {
            this.RegisterUserNameCheckResult = RegisterUserNameCheckResult;
        }
    }

    [SerializableAttribute()]
    [XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class UserNameToken : INotifyPropertyChanged
    {

        private string userNameField;

        private string passwordField;

        private string checkCodeField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [XmlElementAttribute(Order = 0)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
                this.RaisePropertyChanged("UserName");
            }
        }

        /// <remarks/>
        [XmlElementAttribute(Order = 1)]
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
                this.RaisePropertyChanged("Password");
            }
        }

        /// <remarks/>
        [XmlElementAttribute(Order = 2)]
        public string CheckCode
        {
            get
            {
                return this.checkCodeField;
            }
            set
            {
                this.checkCodeField = value;
                this.RaisePropertyChanged("CheckCode");
            }
        }

        /// <remarks/>
        [XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    internal class Passport91ServiceClient : ClientBase<Passport91ServiceInterfaceSoap>, Passport91ServiceInterfaceSoap
    {
        public Passport91ServiceClient()
            : base(new BasicHttpBinding() { SendTimeout = TimeSpan.FromSeconds(30) }, new EndpointAddress(Passport91Service.ServiceUrl))
        {
        }

        RegisterUserInfo_Cop_591UP_WithRegTypeResponse Passport91ServiceInterfaceSoap.RegisterUserInfo_Cop_591UP_WithRegType(RegisterUserInfo_Cop_591UP_WithRegTypeRequest request)
        {
            return base.Channel.RegisterUserInfo_Cop_591UP_WithRegType(request);
        }

        public int RegisterUserInfo_Cop_591UP_WithRegType(UserNameToken UserNameToken, string userName, string password, string nickName, string mobile, string ipAddress, int regType, string checkCode, out uint return_userid)
        {
            RegisterUserInfo_Cop_591UP_WithRegTypeRequest inValue = new RegisterUserInfo_Cop_591UP_WithRegTypeRequest();
            inValue.UserNameToken = UserNameToken;
            inValue.userName = userName;
            inValue.password = password;
            inValue.nickName = nickName;
            inValue.mobile = mobile;
            inValue.ipAddress = ipAddress;
            inValue.regType = regType;
            inValue.checkCode = checkCode;
            RegisterUserInfo_Cop_591UP_WithRegTypeResponse retVal = ((Passport91ServiceInterfaceSoap)(this)).RegisterUserInfo_Cop_591UP_WithRegType(inValue);
            return_userid = retVal.return_userid;
            return retVal.RegisterUserInfo_Cop_591UP_WithRegTypeResult;
        }

        RegisterUserNameCheckResponse Passport91ServiceInterfaceSoap.RegisterUserNameCheck(RegisterUserNameCheckRequest request)
        {
            return base.Channel.RegisterUserNameCheck(request);
        }

        public int RegisterUserNameCheck(UserNameToken UserNameToken, string username, string checkcode)
        {
            RegisterUserNameCheckRequest inValue = new RegisterUserNameCheckRequest();
            inValue.UserNameToken = UserNameToken;
            inValue.username = username;
            inValue.checkcode = checkcode;
            RegisterUserNameCheckResponse retVal = ((Passport91ServiceInterfaceSoap)(this)).RegisterUserNameCheck(inValue);
            return retVal.RegisterUserNameCheckResult;
        }
    }
}
