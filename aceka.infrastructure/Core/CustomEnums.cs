namespace aceka.infrastructure.Core
{
    public static class CustomEnums
    {
        public enum RecortMethodType
        {
            executeScalar,
            executeNonQuery
        }

        public enum ParametreStokkart
        {
            model = 0,
            kumas = 20,
            aksesuar = 21,
            iplik = 22
        }


        public enum OnayLogTipi
        {
            genel_onay,
            malzeme_onay,
            yukleme_onay,
            uretim_onay,
            dikim_onay
        }

        public enum SiparisDetayPivotType
        {
            adet,
            birimfiyat
        }

        //public enum stokkartOnayLogTipi
        //{
        //    genel_onay,
        //    malzeme_onay,
        //    yukleme_onay,
        //    uretim_onay,
        //    dikim_onay
        //}

        //public enum SiparisOnayLogTiplari
        //{
        //    genel_onay,
        //    malzeme_onay,
        //    yukleme_onay,
        //    uretim_onay,
        //    dikim_onay
        //}
    }
}
