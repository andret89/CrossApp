using System;

namespace CrossApp.Models
{

    public class InterventiModel
    {
        public int INT_ID { get; set; }
        public int INT_TIPO { get; set; }
        public string TipoIntervento
        {
            get
            {
                if (INT_TIPO == 1)
                    return "Manutenzione Straordinaria";
                else if (INT_TIPO == 2)
                    return "Manutenzione Ordinaria";
                else if (INT_TIPO == 3)
                    return "Verifica Efficienza Energetica";
                else
                    return "Garanzia";
            }
        }
        public int? INT_IMP_ID { get; set; }
        public int? INT_CI_ID { get; set; }
        public int? INT_MI_ID { get; set; }
        public int? INT_SOC_ID_TECNICO { get; set; }
        public int? INT_SOC_ID_CLIENTE { get; set; }
        public int? INT_SOC_ID_IMPR { get; set; }
        public int? INT_SOC_ID_RESPONSABILE { get; set; }
        public int? INT_TBL_ID_TIPO_RESPONS { get; set; }
        public int? INT_STATO { get; set; }
        public string StatoIntervento
        {
            get
            {
                if (INT_STATO != null)
                {
                    if (INT_STATO == 1)
                        return "Aperto";
                    else if (INT_STATO == 2)
                        return "Assegnato";
                    else
                        return "Chiuso";
                }
                else
                    return String.Empty;
            }
        }
        public DateTime? INT_DT_PREN { get; set; }

        public DateTime INT_DT { get; set; }
        public string FormatData { get { return INT_DT.ToString("dd-MM-yyyy"); } }
        public String FormatOra { get { return INT_DT.ToString("HH:mm"); } }

        public DateTime? INT_DT_CHIUSO { get; set; }

        public DateTime? INT_DT_INVIO_CEE { get; set; }
        public string INT_DESC { get; set; }
        public bool? INT_PRES_DICH_CONF { get; set; }
        public bool PresDichConf
        {
            get { return INT_PRES_DICH_CONF == null ? false : (bool)INT_PRES_DICH_CONF; }
        }
        public bool? INT_PRES_LIB_IMP { get; set; }
        public bool PresLibImp
        {
            get { return INT_PRES_LIB_IMP == null ? false : (bool)INT_PRES_LIB_IMP; }
        }
        public bool? INT_PRES_LIB_MAN_GEN { get; set; }
        public bool PresLibManGen
        {
            get { return INT_PRES_LIB_MAN_GEN == null ? false : (bool)INT_PRES_LIB_MAN_GEN; }
        }
        public bool? INT_LIB_COMPILATO { get; set; }
        public bool PresLibCompilato
        {
            get { return INT_LIB_COMPILATO == null ? false : (bool)INT_LIB_COMPILATO; }
        }
        public bool? INT_LOCALE_IDONEO { get; set; }
        public bool? INT_GENERATORI_IDONEI { get; set; }
        public bool? INT_AP_LIBERE_OSTR { get; set; }

        public bool? INT_AP_DIM_ADEGUATE { get; set; }

        public bool? INT_SCAR_IDONEI { get; set; }
        public bool? INT_SIST_REG_FUNZIONANTI { get; set; }

        public bool? INT_ASSENZA_PERD_COMB { get; set; }
        public bool? INT_TEN_IMP_INT_IDONEA { get; set; }

        public bool? INT_DISP_COM_FUNZIONANTI { get; set; }

        public bool? INT_DISP_SIC_NON_MANOMESSI { get; set; }

        public bool? INT_VALV_SIC { get; set; }

        public bool? INT_SCAMB_PULITO { get; set; }

        public bool? INT_PRES_RIFLUSSO { get; set; }

        public bool? INT_RIS_CONTR_UNI_10389 { get; set; }
        public double? INT_DEPR_CAN_FUMO { get; set; }

        public double? INT_SENS_TEMP_FUMI { get; set; }

        public double? INT_SENS_TEMP_ARIA { get; set; }

        public double? INT_SENS_O2 { get; set; }

        public double? INT_SENS_CO2 { get; set; }

        public int? INT_SENS_BACHARACH_1 { get; set; }
        public int? INT_SENS_BACHARACH_2 { get; set; }
        public int? INT_SENS_BACHARACH_3 { get; set; }
        public double? INT_SENS_CO_CORRETTO { get; set; }

        public double? INT_SENS_REND_COMB { get; set; }

        public double? INT_SENS_REND_MIN { get; set; }

        public int? INT_MOD_TERM { get; set; }

        public bool? INT_ADOZ_VALV { get; set; }
        public bool AdozValvole
        {
            get { return INT_ADOZ_VALV == null ? false : (bool)INT_ADOZ_VALV; }
        }

        public bool? INT_ISOL_RETE { get; set; }
        public bool IsolRete
        {
            get { return INT_ISOL_RETE == null ? false : (bool)INT_ISOL_RETE; }
        }

        public bool? INT_INTR_TRATT_ACS { get; set; }
        public bool IntrTrattACS
        {
            get { return INT_INTR_TRATT_ACS == null ? false : (bool)INT_INTR_TRATT_ACS; }
        }

        public bool? INT_SIST_PROG { get; set; }
        public bool SistProg
        {
            get { return INT_SIST_PROG == null ? false : (bool)INT_SIST_PROG; }
        }

        public string INT_OSSERVAZIONI { get; set; }

        public string INT_RACCOMANDAZIONI { get; set; }

        public string INT_PRESCRIZIONI { get; set; }

        public bool? INT_IMPIANTO_FUNZ { get; set; }
        public bool ImpFunz
        {
            get { return INT_IMPIANTO_FUNZ == null ? false : (bool)INT_IMPIANTO_FUNZ; }
        }

        public DateTime? INT_RACC_INT_DATA { get; set; }

        public DateTime? INT_DT_CONTROLLO { get; set; }

        public DateTime INT_ORA_ARRIVO { get; set; }
        public TimeSpan OraArrivo
        {
            get
            {
                return INT_DT.TimeOfDay;
            }
            set
            {
                INT_ORA_ARRIVO = new DateTime(INT_DT.Year, INT_DT.Month, INT_DT.Day, value.Hours, value.Minutes, value.Seconds);
            }
        }
        public DateTime INT_ORA_PARTENZA { get; set; }
        public TimeSpan OraPartenza
        {
            get
            {
                return INT_DT.TimeOfDay;
            }
            set
            {
                INT_ORA_PARTENZA = new DateTime(INT_DT.Year, INT_DT.Month, INT_DT.Day, value.Hours, value.Minutes, value.Seconds);
            }
        }

        public string INT_IMP_COD_CAT { get; set; }
        public double? INT_IMP_PT_TERM_NOM_MAX { get; set; }
        public int? INT_IMP_ID_COMUNE { get; set; }
        public int? INT_IMP_ID_PROV { get; set; }
        public string INT_IMP_INDIR { get; set; }
        public int? INT_IMP_N_CIV { get; set; }
        public string INT_IMP_PALAZZO { get; set; }
        public int? INT_IMP_SCALA { get; set; }
        public string INT_IMP_INTERNO { get; set; }
        public double? INT_IMP_DUREZZA_ACQ { get; set; }
        public int? INT_IMP_TRATT_RISC { get; set; }
        public int? INT_IMP_TRATT_ACS { get; set; }
        public string INT_CLI_COGNOME { get; set; }
        public string INT_CLI_NOME { get; set; }
        public string INT_CLI_CF { get; set; }
        public string INT_CLI_RAG_SOC { get; set; }
        public string INT_CLI_PI { get; set; }
        public string INT_CLI_INDIR { get; set; }
        public int? INT_CLI_N_CIV { get; set; }
        public int? INT_CLI_ID_COMUNE { get; set; }
        public int? INT_CLI_ID_PROV { get; set; }
        public string INT_IMPR_RAG_SOC { get; set; }
        public string INT_IMPR_PI { get; set; }
        public string INT_IMPR_INDIR { get; set; }
        public int? INT_IMPR_N_CIV { get; set; }
        public int? INT_IMPR_ID_COMUNE { get; set; }
        public int? INT_IMPR_ID_PROV { get; set; }
        public int? INT_CI_PROGRESSIVO { get; set; }
        public DateTime? INT_CI_DT_INSTALL { get; set; }
        public string INT_CI_MATRICOLA { get; set; }
        public string INT_MI_MARCA { get; set; }
        public string INT_MI_MODELLO { get; set; }
        public int? INT_MI_TIPO_GT { get; set; }
        public double? INT_MI_PT_MAX_FOCOLARE { get; set; }
        public double? INT_MI_PT_NOM_UTILE { get; set; }
        public bool? INT_MI_CLIMA_INVERN { get; set; }
        public bool MIClimaInvern
        {
            get { return INT_MI_CLIMA_INVERN == null ? false : (bool)INT_MI_CLIMA_INVERN; }
        }
        public bool? INT_MI_PROD_ACS { get; set; }
        public bool MIProdACS
        {
            get { return INT_MI_PROD_ACS == null ? false : (bool)INT_MI_PROD_ACS; }
        }
        public int? INT_MI_TC_ID { get; set; }
        public string INT_MI_TC_DESC { get; set; }
        public int? INT_MI_MOD_EVAC_FUMI { get; set; }
        public int? IDCliente { get; set; }
        public string Modello { get; set; }
        public string Marca { get; set; }
        public string TipoComponente { get; set; }
        public string ComuneImpianto { get; set; }
        public string SiglaProvImpianto { get; set; }
        public string ComuneCliente { get; set; }
        public string ProvinciaCliente { get; set; }
        public string ComuneImpresa { get; set; }
        public string ProvinciaImpresa { get; set; }
        public string Tecnico { get; set; }
        public InterventiModel()
        {

        }
    }
}
