namespace Caliberweb.Core.Licensing
{
    public class MyTestLicenseCreator : ILicenseCreator<MyTestLicense>
    {
        private readonly int number;
        private readonly string name;

        public MyTestLicenseCreator(int number, string name)
        {
            this.number = number;
            this.name = name;
        }

        public MyTestLicense CreateLicense(ILicense license)
        {
            return new MyTestLicense(license)
            {
                StreetName = name,
                StreetNumber = number
            };
        }

        public void AlterDocument(ILicenseDocument document, MyTestLicense license)
        {}
    }
}