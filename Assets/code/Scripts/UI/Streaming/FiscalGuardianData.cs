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
    public string WrongMessage => _wrongMessage;
    
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
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Draw the total pemasukan and pengeluaran
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Total Pemasukan dan Pengeluaran");
        foreach (var people in ((FiscalGuardianData)target).People)
        {
            EditorGUILayout.LabelField($"Total Pemasukan {people.TotalPemasukan}");
            EditorGUILayout.LabelField($"Total Pengeluaran {people.TotalPengeluaran}");
            EditorGUILayout.LabelField($"Pajak Bulanan {people.PajakBulanan}");
            EditorGUILayout.LabelField($"Cicilan {people.CicilanKredit}");
            EditorGUILayout.LabelField($"Is Shadow {people.IsShadow}");
            EditorGUILayout.Space();
        }

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
            // sort the dates
            dts.Sort();
            // ensure the dates are unique
            for (int i = 1; i < dts.Count; i++)
            {
                if (dts[i] == dts[i - 1])
                {
                    dts[i] = dts[i].AddDays(1);
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

            foreach (var people in ((FiscalGuardianData)target).People)
            {
                foreach (var catatan in people.Catatan)
                {
                    catatan.Tanggal = dts[0].ToString("dd-MM-yyyy");
                    dts.RemoveAt(0);
                }
            }
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
        }

        if(GUILayout.Button("Randomize Cicilan Kredit"))
        {
            foreach (var people in ((FiscalGuardianData)target).People)
            {
                people.CicilanKredit = UnityEngine.Random.Range(0, 700) * 1000;
            }
        }

        if(GUILayout.Button("Randomize Id"))
        {
            foreach (var people in ((FiscalGuardianData)target).People)
            {
                people.Id = UnityEngine.Random.Range(0, 16);
            }
        }
    }
    private System.Random gen = new System.Random();
    DateTime RandomDay2024()
    {
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;           
        DateTime dt = start.AddDays(gen.Next(range));
        return new DateTime(2024,dt.Month,dt.Day);
    }
}
#endif