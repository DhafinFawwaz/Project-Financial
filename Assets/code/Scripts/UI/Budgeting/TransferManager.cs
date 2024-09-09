using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferManager : MonoBehaviour
{
    [SerializeField] TextRupiahAnimation _jajan;
    [SerializeField] TextRupiahAnimation _kebutuhan;
    [SerializeField] TextRupiahAnimation _tabungan;
    [SerializeField] TextRupiahAnimation _streaming;

    void Start()
    {
        SetToDebit();
    }

    public void SetToDebit()
    {
        


        _jajan.SetValues(Save.Data.DesireMoney, 0);
        _kebutuhan.SetValues(Save.Data.NeedsMoney, 0);
        _tabungan.SetValues(Save.Data.DebitTabunganMoney, Save.Data.DebitTabunganMoney + Save.Data.NeedsMoney + Save.Data.DesireMoney);


        Save.Data.DebitTabunganMoney += Save.Data.NeedsMoney + Save.Data.DesireMoney;
        Save.Data.DesireMoney = 0;
        Save.Data.NeedsMoney = 0;
    }

    public void SetToStreaming()
    {
        long total = Save.Data.GetSalaryFromToday();
        // Debug.Log("total salary: " + total);
        _streaming.SetValues(total, 0);

        Save.Data.DesireMoney = total/3;
        Save.Data.NeedsMoney = total/3;
        Save.Data.DebitMoney = total/3;

        _jajan.SetValues(0, Save.Data.DesireMoney);
        _kebutuhan.SetValues(0, Save.Data.NeedsMoney);
        _tabungan.SetPrefix(Save.Data.DebitTabunganMoney.ToStringRupiahFormat() + " + ");
        _tabungan.SetValues(0, Save.Data.DebitMoney);
    }
}
