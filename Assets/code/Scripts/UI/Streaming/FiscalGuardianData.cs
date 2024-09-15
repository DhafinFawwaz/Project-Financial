using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "FiscalGuardianData", menuName = "FiscalGuardianData", order = 1)]
public class FiscalGuardianData : ScriptableObject
{
    [SerializeField] FiscalGuardianPeople[] _people;
    public FiscalGuardianPeople[] People => _people;
    public int GetShadowCount() {
        int shadowCount = 0;
        foreach (var people in _people)
        {
            if(people.IsShadow)
            {
                shadowCount++;
            }
        }
        return shadowCount;
    }

    public void RandomizeId()
    {
        foreach (var people in _people)
        {
            people.Id = UnityEngine.Random.Range(0, 16);
        }
    }
}

[System.Serializable]
public class FiscalGuardianPeople
{
    [SerializeField] int _id;
    [SerializeField] long _cicilanKredit;
    [TextArea]
    [SerializeField] string _wrongMessage;
    [SerializeField] Catatan[] _catatan;
    public Catatan[] Catatan => _catatan;
    

    public int Id {get => _id; set => _id = value;}
    public long CicilanKredit {get => _cicilanKredit; set => _cicilanKredit = value;}
    public string WrongMessage {get => _wrongMessage; set => _wrongMessage = value;}
    
    public long TotalPemasukan {get {
        long total = 0;
        foreach (var catatan in _catatan)
            total += catatan.Pemasukan;
        return total;
    }}

    public long TotalPengeluaran {get {
        long total = 0;
        foreach (var catatan in _catatan)
            total += catatan.Pengeluaran;
        return total;
    }}

    public long PajakBulanan => (long)(0.03f * TotalPemasukan);

    public bool IsShadow {get {
        return TotalPemasukan < TotalPengeluaran + CicilanKredit + PajakBulanan;
    }}
}

[System.Serializable]
public class Catatan
{
    [SerializeField] string _tanggal;
    [SerializeField] long _pemasukan;
    [SerializeField] long _pengeluaran;

    public string Tanggal {get => _tanggal; set => _tanggal = value;}
    public long Pemasukan {get => _pemasukan; set => _pemasukan = value;}
    public long Pengeluaran {get => _pengeluaran; set => _pengeluaran = value;}
}

[System.Serializable]
public class People
{
    [SerializeField] Sprite _face;
    [SerializeField] Sprite _ktp;
    public Sprite Face => _face;
    public Sprite KTP => _ktp;
}


// draw Author and Comment fields in the inspector side by side without label
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Catatan))]
public class CatatanDrawer : PropertyDrawer
{
    float space = 2;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        Rect tanggalRect = new Rect(position.x, position.y, position.width / 3 - space, position.height);
        Rect pemasukanRect = new Rect(position.x + position.width / 3, position.y, position.width / 3 - space, position.height);
        Rect pengeluaranRect = new Rect(position.x + 2 * position.width / 3, position.y, position.width / 3 - space, position.height);
        EditorGUI.PropertyField(tanggalRect, property.FindPropertyRelative("_tanggal"), GUIContent.none);
        EditorGUI.PropertyField(pemasukanRect, property.FindPropertyRelative("_pemasukan"), GUIContent.none);
        EditorGUI.PropertyField(pengeluaranRect, property.FindPropertyRelative("_pengeluaran"), GUIContent.none);
    }
}

#endif


// add button to FiscalGuardianData
#if UNITY_EDITOR
[CustomEditor(typeof(FiscalGuardianData))]
public class FiscalGuardianDataEditor : Editor
{
    List<DateTime> SortDateTime(List<DateTime> dt)
    {
        // dt.Sort((a, b) => a.Day.CompareTo(b.Day));
        return dt;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Draw the total pemasukan and pengeluaran
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Total Pemasukan dan Pengeluaran");
        foreach (var people in ((FiscalGuardianData)target).People)
        {
            EditorGUILayout.LabelField($"Total Pemasukan {people.TotalPemasukan}");
            EditorGUILayout.LabelField($"Total Pengeluaran {people.TotalPengeluaran + people.PajakBulanan + people.CicilanKredit}");
            
            EditorGUILayout.LabelField($"Pengeluaran {people.TotalPengeluaran}");
            EditorGUILayout.LabelField($"Pajak Bulanan {people.PajakBulanan}");
            EditorGUILayout.LabelField($"Cicilan {people.CicilanKredit}");
            EditorGUILayout.LabelField($"Is Shadow {people.IsShadow}");
            EditorGUILayout.LabelField($"Id {people.Id}");
            EditorGUILayout.Space();
        }

        EditorGUILayout.LabelField($"Shadow: {((FiscalGuardianData)target).GetShadowCount()}");


        if (GUILayout.Button("Randomize Date"))
        {
            List<DateTime> dts = new List<DateTime>();
            foreach (var people in ((FiscalGuardianData)target).People)
            {
                foreach (var catatan in people.Catatan)
                {
                    dts.Add(RandomDay2024());
                }
            }

            // ensure the month is all same
            int month = dts[0].Month;
            for(int i = 0; i < dts.Count; i++)
            {
                if(dts[i].Month != month)
                {
                    dts[i] = new DateTime(dts[i].Year, month, dts[i].Day);
                }
            }

            // sort the dates
            dts = SortDateTime(dts);



            // ensure the dates are unique
            for (int i = 1; i < dts.Count; i++)
            {
                if (dts[i] == dts[i - 1])
                {
                    dts[i] = dts[i].AddDays(1);
                }
            }

            foreach (var people in ((FiscalGuardianData)target).People)
            {
                // foreach (var catatan in people.Catatan)
                // {
                //     catatan.Tanggal = dts[0].ToString("dd-MM-yyyy");
                //     dts.RemoveAt(0);
                // }
                for(int i = 0; i < people.Catatan.Length; i++)
                {
                    people.Catatan[i].Tanggal = dts[i].ToString("dd-MM-yyyy");
                }
            }
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        if(GUILayout.Button("Randomize Pemasukan"))
        {
            foreach (var people in ((FiscalGuardianData)target).People)
            {
                foreach (var catatan in people.Catatan)
                {
                    long val = UnityEngine.Random.Range(0, 200);
                    catatan.Pemasukan = val * 1000;
                }
            }
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        if(GUILayout.Button("Randomize Pengeluaran"))
        {
            foreach (var people in ((FiscalGuardianData)target).People)
            {
                foreach (var catatan in people.Catatan)
                {
                    long val = UnityEngine.Random.Range(0, 120);
                    catatan.Pengeluaran = val * 1000;
                }
            }
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        if(GUILayout.Button("Randomize Cicilan Kredit"))
        {
            foreach (var people in ((FiscalGuardianData)target).People)
            {
                people.CicilanKredit = UnityEngine.Random.Range(0, 700) * 1000;
            }
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        if(GUILayout.Button("Randomize Id"))
        {
            ((FiscalGuardianData)target).RandomizeId();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
        if(GUILayout.Button("Randomize Message"))
        {
            string[] messageList = new string[]{
                "APA??? Ini tidak adil!",
                "hei apa-apaan ini???! Keuanganku jelas-jelas bagus",
                "pegawai baru ini tidak bisa dipercaya!!! Awas saja kau!",
                "sial banget hari ini, ketemu pegawai tidak becus seperti kamu!!",
                "APA SALAHKUUUUU??",
                "Pemerintah memang tidak bisa dipercaya!",
            };
            foreach (var people in ((FiscalGuardianData)target).People)
            {
                people.WrongMessage = messageList[UnityEngine.Random.Range(0, messageList.Length)];
            }

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

    }
    private System.Random gen = new System.Random();
    DateTime RandomDay2024()
    {
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;           
        DateTime dt = start.AddDays(gen.Next(range));
        return new DateTime(2024,dt.Month,UnityEngine.Random.Range(1, 29));
    }
}
#endif