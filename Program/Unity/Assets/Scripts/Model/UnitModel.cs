using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModel : Model
{
    public class Unit
    {
        public int index { get; private set; }
        public string unitName { get; private set; }
        public string resourceName { get; private set; }
        public int unitType { get; private set; }
        public int baseGrade { get; private set; }
        public int baseLevel { get; private set; }
        public int maxGrade { get; private set; }
        public int maxLevel { get; private set; }
        public int baseHP { get; private set; }
        public int baseDamage { get; private set; }
        public int lvHP { get; private set; }
        public int lvDamage { get; private set; }
        public int gHP { get; private set; }
        public int gDamage { get; private set; }

        public Unit(int index, string unitName, string resourceName, int unitType, int baseGrade, int baseLevel, int maxGrade, int maxLevel, int baseHP, int baseDamage, int lvHP, int lvDamage, int gHP, int gDamage)
        {
            this.index = index;
            this.unitName = unitName;
            this.resourceName = resourceName;
            this.unitType = unitType;
            this.baseGrade = baseGrade;
            this.baseLevel = baseLevel;
            this.maxGrade = maxGrade;
            this.maxLevel = maxLevel;
            this.baseHP = baseHP;
            this.baseDamage = baseDamage;
            this.lvHP = lvHP;
            this.lvDamage = lvDamage;
            this.gHP = gHP;
            this.gDamage = gDamage;
        }
    }

    private List<Unit> _unitTableList = new List<Unit>( );
    public List<Unit> unitTable { get { return _unitTableList; } }

    public void Setup( )
    {
        CSVReader reader = CSVReader.Load("Table/UnitTable");

        int maxCount = reader.rowCount;
        int idx = 0;
        CSVReader.Row row = null;
        Unit unit = null;
        for (int i = 1; i < maxCount; i++)
        {
            row = reader.GetRow(i);

            idx = 0;

            unit = new Unit(
                row.GetInt(idx++),
                row.GetString(idx++),
                row.GetString(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++),
                row.GetInt(idx++)
            );

            _unitTableList.Add(unit);
        }
    }
}